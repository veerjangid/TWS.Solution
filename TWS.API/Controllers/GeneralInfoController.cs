using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWS.Core.DTOs.Request.GeneralInfo;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.GeneralInfo;
using TWS.Core.Interfaces.IServices;

namespace TWS.API.Controllers
{
    /// <summary>
    /// Controller for General Info management operations
    /// Handles Individual, Joint, IRA, Trust, and Entity general information
    /// </summary>
    [ApiController]
    [Route("api/general-info")]
    public class GeneralInfoController : ControllerBase
    {
        private readonly IGeneralInfoService _generalInfoService;
        private readonly ILogger<GeneralInfoController> _logger;
        private readonly IWebHostEnvironment _environment;

        public GeneralInfoController(
            IGeneralInfoService generalInfoService,
            ILogger<GeneralInfoController> logger,
            IWebHostEnvironment environment)
        {
            _generalInfoService = generalInfoService;
            _logger = logger;
            _environment = environment;
        }

        /// <summary>
        /// Save or update Individual General Info
        /// </summary>
        /// <param name="request">Individual general info request</param>
        /// <returns>Created general info response</returns>
        [HttpPost("individual")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<GeneralInfoResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SaveIndividualGeneralInfo([FromBody] SaveIndividualGeneralInfoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for SaveIndividualGeneralInfo");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                _logger.LogInformation("Saving Individual General Info for InvestorProfileId {InvestorProfileId}", request.InvestorProfileId);
                var response = await _generalInfoService.SaveIndividualGeneralInfoAsync(request);

                if (response.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Individual General Info for InvestorProfileId {InvestorProfileId}", request.InvestorProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Save or update Joint General Info
        /// </summary>
        /// <param name="request">Joint general info request</param>
        /// <returns>Created general info response</returns>
        [HttpPost("joint")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<GeneralInfoResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SaveJointGeneralInfo([FromBody] SaveJointGeneralInfoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for SaveJointGeneralInfo");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                _logger.LogInformation("Saving Joint General Info for InvestorProfileId {InvestorProfileId}", request.InvestorProfileId);
                var response = await _generalInfoService.SaveJointGeneralInfoAsync(request);

                if (response.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Joint General Info for InvestorProfileId {InvestorProfileId}", request.InvestorProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Add Joint Account Holder to existing Joint General Info
        /// </summary>
        /// <param name="request">Joint account holder request</param>
        /// <returns>Created joint account holder response</returns>
        [HttpPost("joint/account-holder")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<JointAccountHolderResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddJointAccountHolder([FromBody] AddJointAccountHolderRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for AddJointAccountHolder");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                _logger.LogInformation("Adding Joint Account Holder for JointGeneralInfoId {JointGeneralInfoId}", request.JointGeneralInfoId);
                var response = await _generalInfoService.AddJointAccountHolderAsync(request);

                if (response.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Joint Account Holder for JointGeneralInfoId {JointGeneralInfoId}", request.JointGeneralInfoId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Save or update IRA General Info
        /// </summary>
        /// <param name="request">IRA general info request</param>
        /// <returns>Created general info response</returns>
        [HttpPost("ira")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<GeneralInfoResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SaveIRAGeneralInfo([FromBody] SaveIRAGeneralInfoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for SaveIRAGeneralInfo");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate IRA Account Type is between 1-5
                if (request.AccountType < 1 || request.AccountType > 5)
                {
                    _logger.LogWarning("Invalid IRA AccountType {AccountType} for InvestorProfileId {InvestorProfileId}", request.AccountType, request.InvestorProfileId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "IRA AccountType must be between 1 and 5 (Traditional IRA, Roth IRA, SEP IRA, Inherited IRA, Inherited Roth IRA)",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                _logger.LogInformation("Saving IRA General Info for InvestorProfileId {InvestorProfileId}", request.InvestorProfileId);
                var response = await _generalInfoService.SaveIRAGeneralInfoAsync(request);

                if (response.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving IRA General Info for InvestorProfileId {InvestorProfileId}", request.InvestorProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Save or update Trust General Info
        /// </summary>
        /// <param name="request">Trust general info request</param>
        /// <returns>Created general info response</returns>
        [HttpPost("trust")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<GeneralInfoResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SaveTrustGeneralInfo([FromBody] SaveTrustGeneralInfoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for SaveTrustGeneralInfo");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                _logger.LogInformation("Saving Trust General Info for InvestorProfileId {InvestorProfileId}", request.InvestorProfileId);
                var response = await _generalInfoService.SaveTrustGeneralInfoAsync(request);

                if (response.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Trust General Info for InvestorProfileId {InvestorProfileId}", request.InvestorProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Add Trust Grantor to existing Trust General Info
        /// </summary>
        /// <param name="request">Trust grantor request</param>
        /// <returns>Created trust grantor response</returns>
        [HttpPost("trust/grantor")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<TrustGrantorResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddTrustGrantor([FromBody] AddTrustGrantorRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for AddTrustGrantor");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                _logger.LogInformation("Adding Trust Grantor for TrustGeneralInfoId {TrustGeneralInfoId}", request.TrustGeneralInfoId);
                var response = await _generalInfoService.AddTrustGrantorAsync(request);

                if (response.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Trust Grantor for TrustGeneralInfoId {TrustGeneralInfoId}", request.TrustGeneralInfoId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Save or update Entity General Info
        /// </summary>
        /// <param name="request">Entity general info request</param>
        /// <returns>Created general info response</returns>
        [HttpPost("entity")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<GeneralInfoResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SaveEntityGeneralInfo([FromBody] SaveEntityGeneralInfoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for SaveEntityGeneralInfo");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                _logger.LogInformation("Saving Entity General Info for InvestorProfileId {InvestorProfileId}", request.InvestorProfileId);
                var response = await _generalInfoService.SaveEntityGeneralInfoAsync(request);

                if (response.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Entity General Info for InvestorProfileId {InvestorProfileId}", request.InvestorProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Add Entity Equity Owner to existing Entity General Info
        /// </summary>
        /// <param name="request">Entity equity owner request</param>
        /// <returns>Created entity equity owner response</returns>
        [HttpPost("entity/equity-owner")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<EntityEquityOwnerResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddEntityEquityOwner([FromBody] AddEntityEquityOwnerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for AddEntityEquityOwner");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                _logger.LogInformation("Adding Entity Equity Owner for EntityGeneralInfoId {EntityGeneralInfoId}", request.EntityGeneralInfoId);
                var response = await _generalInfoService.AddEntityEquityOwnerAsync(request);

                if (response.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Entity Equity Owner for EntityGeneralInfoId {EntityGeneralInfoId}", request.EntityGeneralInfoId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Get General Info by Investor Profile ID
        /// </summary>
        /// <param name="investorProfileId">Investor Profile ID</param>
        /// <returns>General info response with type-specific data</returns>
        [HttpGet("investor/{investorProfileId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<GeneralInfoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetGeneralInfoByInvestorProfile(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Retrieving General Info for InvestorProfileId {InvestorProfileId}", investorProfileId);
                var response = await _generalInfoService.GetGeneralInfoByInvestorProfileIdAsync(investorProfileId);

                if (response.Success)
                {
                    return Ok(response);
                }

                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving General Info for InvestorProfileId {InvestorProfileId}", investorProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Upload General Info Document (Driver License or W9)
        /// </summary>
        /// <param name="investorProfileId">Investor Profile ID</param>
        /// <param name="documentType">Document type (DriverLicense or W9)</param>
        /// <param name="file">Document file</param>
        /// <returns>Document upload response with file path</returns>
        [HttpPost("upload-document")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UploadDocument([FromForm] int investorProfileId, [FromForm] string documentType, [FromForm] IFormFile file)
        {
            try
            {
                // Validate parameters
                if (file == null || file.Length == 0)
                {
                    _logger.LogWarning("File is empty for InvestorProfileId {InvestorProfileId}", investorProfileId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "File is required",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate file size (max 10MB)
                const long maxFileSize = 10 * 1024 * 1024; // 10MB
                if (file.Length > maxFileSize)
                {
                    _logger.LogWarning("File size {FileSize} exceeds maximum for InvestorProfileId {InvestorProfileId}", file.Length, investorProfileId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "File size exceeds maximum limit of 10MB",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate document type
                if (string.IsNullOrWhiteSpace(documentType) ||
                    (documentType != "DriverLicense" && documentType != "W9"))
                {
                    _logger.LogWarning("Invalid document type {DocumentType} for InvestorProfileId {InvestorProfileId}", documentType, investorProfileId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Document type must be either 'DriverLicense' or 'W9'",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate file extension
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    _logger.LogWarning("Invalid file extension {Extension} for InvestorProfileId {InvestorProfileId}", fileExtension, investorProfileId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Only PDF, JPG, JPEG, and PNG files are allowed",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Create upload directory
                var uploadDirectory = Path.Combine(_environment.WebRootPath ?? _environment.ContentRootPath, "uploads", "general-info", investorProfileId.ToString());
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                    _logger.LogInformation("Created upload directory {Directory}", uploadDirectory);
                }

                // Generate unique file name
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                var fileName = $"{documentType}_{timestamp}{fileExtension}";
                var filePath = Path.Combine(uploadDirectory, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                _logger.LogInformation("Document uploaded successfully for InvestorProfileId {InvestorProfileId}, DocumentType {DocumentType}, Path {Path}",
                    investorProfileId, documentType, filePath);

                // Return relative path for API response
                var relativePath = $"/uploads/general-info/{investorProfileId}/{fileName}";

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Document uploaded successfully",
                    Data = new
                    {
                        DocumentId = 0, // Will be replaced with actual document ID once database implementation is complete
                        FilePath = relativePath
                    },
                    StatusCode = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading document for InvestorProfileId {InvestorProfileId}, DocumentType {DocumentType}",
                    investorProfileId, documentType);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while uploading the document",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}