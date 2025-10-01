using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.PersonalFinancialStatement;
using TWS.Core.Interfaces.IServices;

namespace TWS.API.Controllers
{
    /// <summary>
    /// Controller for Personal Financial Statement document operations.
    /// Handles upload, retrieval, and deletion of PFS documents.
    /// Reference: APIDoc.md Section 8, BusinessRequirement.md Section 9
    /// </summary>
    [ApiController]
    [Route("api/personal-financial-statement")]
    public class PersonalFinancialStatementController : ControllerBase
    {
        private readonly IPersonalFinancialStatementService _pfsService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<PersonalFinancialStatementController> _logger;

        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB
        private static readonly string[] AllowedExtensions = { ".pdf" };

        public PersonalFinancialStatementController(
            IPersonalFinancialStatementService pfsService,
            IWebHostEnvironment webHostEnvironment,
            ILogger<PersonalFinancialStatementController> logger)
        {
            _pfsService = pfsService ?? throw new ArgumentNullException(nameof(pfsService));
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Upload a Personal Financial Statement document for an investor profile.
        /// Only PDF files are allowed (max 10MB).
        /// Replaces existing PFS if one exists.
        /// </summary>
        /// <param name="investorProfileId">The ID of the investor profile</param>
        /// <param name="file">The PDF file to upload</param>
        /// <returns>Uploaded PFS details</returns>
        [HttpPost("upload")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PFSResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UploadPFS([FromForm] int investorProfileId, [FromForm] IFormFile file)
        {
            try
            {
                _logger.LogInformation("Uploading PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                // Validate investorProfileId
                if (investorProfileId <= 0)
                {
                    _logger.LogWarning("Invalid InvestorProfileId: {InvestorProfileId}", investorProfileId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "InvestorProfileId must be a positive integer",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate file presence
                if (file == null || file.Length == 0)
                {
                    _logger.LogWarning("No file provided for PFS upload");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "No file provided",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate file size
                if (file.Length > MaxFileSize)
                {
                    _logger.LogWarning("File size exceeds limit: {FileSize} bytes", file.Length);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"File size exceeds maximum limit of {MaxFileSize / (1024 * 1024)}MB",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate file extension
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(fileExtension))
                {
                    _logger.LogWarning("Invalid file extension: {FileExtension}", fileExtension);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Only PDF files are allowed",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Create upload directory path
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "pfs", investorProfileId.ToString());

                // Create directory if it doesn't exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                    _logger.LogInformation("Created directory: {Directory}", uploadsFolder);
                }

                // Generate unique filename
                var fileName = $"PFS_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Save file to disk
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                _logger.LogInformation("File saved to: {FilePath}", filePath);

                // Save to database
                var response = await _pfsService.UploadPFSAsync(investorProfileId, filePath);

                if (response.Success)
                {
                    return StatusCode(response.StatusCode, response);
                }
                else
                {
                    // If service call failed, delete the uploaded file
                    if (System.IO.File.Exists(filePath))
                    {
                        try
                        {
                            System.IO.File.Delete(filePath);
                            _logger.LogInformation("Deleted file after service failure: {FilePath}", filePath);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to delete file after service failure: {FilePath}", filePath);
                        }
                    }

                    return StatusCode(response.StatusCode, response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while uploading the Personal Financial Statement",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Retrieve the Personal Financial Statement for a specific investor profile.
        /// </summary>
        /// <param name="investorProfileId">The ID of the investor profile</param>
        /// <returns>PFS details</returns>
        [HttpGet("investor/{investorProfileId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PFSResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetByInvestorProfileId(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Retrieving PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                if (investorProfileId <= 0)
                {
                    _logger.LogWarning("Invalid InvestorProfileId: {InvestorProfileId}", investorProfileId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "InvestorProfileId must be a positive integer",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                var response = await _pfsService.GetByInvestorProfileIdAsync(investorProfileId);

                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the Personal Financial Statement",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Delete the Personal Financial Statement for a specific investor profile.
        /// Removes both the database record and the physical file.
        /// </summary>
        /// <param name="investorProfileId">The ID of the investor profile</param>
        /// <returns>Success status</returns>
        [HttpDelete("{investorProfileId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeletePFS(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Deleting PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                if (investorProfileId <= 0)
                {
                    _logger.LogWarning("Invalid InvestorProfileId: {InvestorProfileId}", investorProfileId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "InvestorProfileId must be a positive integer",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                var response = await _pfsService.DeletePFSAsync(investorProfileId);

                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while deleting the Personal Financial Statement",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Download the Personal Financial Statement PDF file.
        /// </summary>
        /// <param name="investorProfileId">The ID of the investor profile</param>
        /// <returns>PDF file stream</returns>
        [HttpGet("download/{investorProfileId}")]
        [Authorize]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DownloadPFS(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Downloading PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                if (investorProfileId <= 0)
                {
                    _logger.LogWarning("Invalid InvestorProfileId: {InvestorProfileId}", investorProfileId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "InvestorProfileId must be a positive integer",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                var response = await _pfsService.GetByInvestorProfileIdAsync(investorProfileId);

                if (!response.Success || response.Data == null)
                {
                    return StatusCode(response.StatusCode, response);
                }

                var filePath = response.Data.FilePath;

                if (!System.IO.File.Exists(filePath))
                {
                    _logger.LogWarning("PFS file not found at path: {FilePath}", filePath);
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Personal Financial Statement file not found",
                        StatusCode = StatusCodes.Status404NotFound
                    });
                }

                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                var fileName = Path.GetFileName(filePath);
                _logger.LogInformation("Returning PFS file: {FileName}", fileName);

                return File(memory, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while downloading the Personal Financial Statement",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}