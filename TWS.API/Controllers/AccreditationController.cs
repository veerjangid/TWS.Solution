using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TWS.Core.Constants;
using TWS.Core.DTOs.Request.Accreditation;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Accreditation;
using TWS.Core.Interfaces.IServices;

namespace TWS.API.Controllers
{
    /// <summary>
    /// Controller for Accreditation management operations
    /// Handles investor accreditation submission, document upload, and verification by Operations Team
    /// </summary>
    [ApiController]
    [Route("api/accreditation")]
    public class AccreditationController : ControllerBase
    {
        private readonly IAccreditationService _accreditationService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<AccreditationController> _logger;

        public AccreditationController(
            IAccreditationService accreditationService,
            IWebHostEnvironment webHostEnvironment,
            ILogger<AccreditationController> logger)
        {
            _accreditationService = accreditationService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        /// <summary>
        /// Save or update investor accreditation information
        /// Creates new accreditation record or updates existing one
        /// </summary>
        /// <param name="request">Accreditation save request</param>
        /// <returns>Created or updated accreditation response</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<AccreditationResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<AccreditationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SaveAccreditation([FromBody] SaveAccreditationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for SaveAccreditation");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate license fields for AccreditationType = 4 (LicensesAndCertifications)
                if (request.AccreditationType == 4)
                {
                    if (string.IsNullOrWhiteSpace(request.LicenseNumber))
                    {
                        _logger.LogWarning("License number is required for AccreditationType 4. InvestorProfileId: {InvestorProfileId}",
                            request.InvestorProfileId);
                        return BadRequest(new ApiResponse<object>
                        {
                            Success = false,
                            Message = "License number is required for Licenses and Certifications accreditation type",
                            StatusCode = StatusCodes.Status400BadRequest
                        });
                    }

                    if (string.IsNullOrWhiteSpace(request.StateLicenseHeld))
                    {
                        _logger.LogWarning("State license held is required for AccreditationType 4. InvestorProfileId: {InvestorProfileId}",
                            request.InvestorProfileId);
                        return BadRequest(new ApiResponse<object>
                        {
                            Success = false,
                            Message = "State license held is required for Licenses and Certifications accreditation type",
                            StatusCode = StatusCodes.Status400BadRequest
                        });
                    }
                }

                _logger.LogInformation("Saving Accreditation for InvestorProfileId {InvestorProfileId}, Type: {AccreditationType}",
                    request.InvestorProfileId, request.AccreditationType);
                var response = await _accreditationService.SaveAccreditationAsync(request);

                if (response.Success)
                {
                    // Return 201 Created for new records, 200 OK for updates
                    var statusCode = response.Message?.Contains("created", StringComparison.OrdinalIgnoreCase) == true
                        ? StatusCodes.Status201Created
                        : StatusCodes.Status200OK;
                    return StatusCode(statusCode, response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Accreditation for InvestorProfileId {InvestorProfileId}", request.InvestorProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Upload accreditation document for investor
        /// Supports PDF, JPG, JPEG, PNG files up to 10MB
        /// </summary>
        /// <param name="investorAccreditationId">Investor accreditation ID</param>
        /// <param name="documentType">Type of document (Tax Return, Pay Stub, Bank Statement, CPA Letter, License, etc.)</param>
        /// <param name="file">Document file to upload</param>
        /// <returns>Created accreditation document response</returns>
        [HttpPost("upload-document")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<AccreditationDocumentResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UploadAccreditationDocument(
            [FromForm] int investorAccreditationId,
            [FromForm] string documentType,
            [FromForm] IFormFile file)
        {
            try
            {
                // Validate investorAccreditationId
                if (investorAccreditationId <= 0)
                {
                    _logger.LogWarning("Invalid InvestorAccreditationId: {InvestorAccreditationId}", investorAccreditationId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Invalid investor accreditation ID",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate document type
                if (string.IsNullOrWhiteSpace(documentType))
                {
                    _logger.LogWarning("Document type is required. InvestorAccreditationId: {InvestorAccreditationId}", investorAccreditationId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Document type is required",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate file
                if (file == null || file.Length == 0)
                {
                    _logger.LogWarning("No file uploaded. InvestorAccreditationId: {InvestorAccreditationId}", investorAccreditationId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "File is required",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate file size (max 10MB = 10485760 bytes)
                const long maxFileSize = 10485760;
                if (file.Length > maxFileSize)
                {
                    _logger.LogWarning("File size exceeds 10MB. Size: {FileSize}, InvestorAccreditationId: {InvestorAccreditationId}",
                        file.Length, investorAccreditationId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "File size cannot exceed 10MB",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate file extension
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    _logger.LogWarning("Invalid file type. Extension: {Extension}, InvestorAccreditationId: {InvestorAccreditationId}",
                        fileExtension, investorAccreditationId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Only PDF, JPG, JPEG, and PNG files are allowed",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Create directory path
                var uploadsPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "accreditation", investorAccreditationId.ToString());
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                    _logger.LogInformation("Created directory: {UploadPath}", uploadsPath);
                }

                // Generate unique filename: {documentType}_{timestamp}.{extension}
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                var sanitizedDocumentType = string.Join("_", documentType.Split(Path.GetInvalidFileNameChars()));
                var fileName = $"{sanitizedDocumentType}_{timestamp}{fileExtension}";
                var filePath = Path.Combine(uploadsPath, fileName);

                // Save file to disk
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                _logger.LogInformation("File uploaded successfully. Path: {FilePath}, InvestorAccreditationId: {InvestorAccreditationId}",
                    filePath, investorAccreditationId);

                // Save document record via service
                var response = await _accreditationService.UploadDocumentAsync(investorAccreditationId, documentType, filePath);

                if (response.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, response);
                }

                // If service returns error, delete the uploaded file
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    _logger.LogWarning("Deleted uploaded file due to service error. Path: {FilePath}", filePath);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading accreditation document for InvestorAccreditationId {InvestorAccreditationId}",
                    investorAccreditationId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Verify investor accreditation (Operations Team only)
        /// Approve or reject accreditation with notes
        /// </summary>
        /// <param name="id">Investor accreditation ID</param>
        /// <param name="request">Verification request with approval status and notes</param>
        /// <returns>Updated accreditation response</returns>
        [HttpPut("{id}/verify")]
        [Authorize(Roles = RoleConstants.Advisor + "," + RoleConstants.OperationsTeam)]
        [ProducesResponseType(typeof(ApiResponse<AccreditationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> VerifyAccreditation(int id, [FromBody] VerifyAccreditationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for VerifyAccreditation");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Extract verifiedByUserId from JWT claims
                var verifiedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(verifiedByUserId))
                {
                    _logger.LogWarning("User ID not found in claims for verification. InvestorAccreditationId: {Id}", id);
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "User authentication failed",
                        StatusCode = StatusCodes.Status401Unauthorized
                    });
                }

                _logger.LogInformation("Verifying Accreditation Id: {Id}, VerifiedBy: {VerifiedByUserId}, IsApproved: {IsApproved}",
                    id, verifiedByUserId, request.IsApproved);

                var response = await _accreditationService.VerifyAccreditationAsync(id, verifiedByUserId, request);

                if (response.Success)
                {
                    return Ok(response);
                }

                // Return 404 if not found
                if (response.Message?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return NotFound(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying Accreditation Id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Get accreditation information by investor profile ID
        /// Returns complete accreditation with all uploaded documents
        /// </summary>
        /// <param name="investorProfileId">Investor Profile ID</param>
        /// <returns>Accreditation response with documents</returns>
        [HttpGet("investor/{investorProfileId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<AccreditationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAccreditationByInvestorProfile(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Retrieving Accreditation for InvestorProfileId {InvestorProfileId}", investorProfileId);
                var response = await _accreditationService.GetByInvestorProfileIdAsync(investorProfileId);

                if (response.Success)
                {
                    return Ok(response);
                }

                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Accreditation for InvestorProfileId {InvestorProfileId}", investorProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Delete accreditation document
        /// Removes document record and file from storage
        /// </summary>
        /// <param name="documentId">Accreditation document ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("document/{documentId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteAccreditationDocument(int documentId)
        {
            try
            {
                _logger.LogInformation("Deleting Accreditation Document Id: {DocumentId}", documentId);
                var response = await _accreditationService.DeleteDocumentAsync(documentId);

                if (response.Success)
                {
                    return Ok(response);
                }

                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Accreditation Document Id: {DocumentId}", documentId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}