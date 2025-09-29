using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TWS.Core.DTOs.Request.Investor;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Investor;
using TWS.Core.Interfaces.IServices;

namespace TWS.API.Controllers
{
    /// <summary>
    /// Controller for investor profile management and type selection operations
    /// </summary>
    [ApiController]
    [Route("api/investor")]
    public class InvestorController : ControllerBase
    {
        private readonly IInvestorService _investorService;
        private readonly ILogger<InvestorController> _logger;

        public InvestorController(
            IInvestorService investorService,
            ILogger<InvestorController> logger)
        {
            _investorService = investorService;
            _logger = logger;
        }

        /// <summary>
        /// Select Individual investor type and create profile
        /// </summary>
        /// <param name="request">Individual investor type selection request</param>
        /// <returns>Created investor profile</returns>
        [HttpPost("select-type/individual")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<InvestorProfileResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SelectIndividualType([FromBody] SelectInvestorTypeIndividualRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for SelectIndividualType");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                var userId = GetUserIdFromClaims();
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID not found in claims");
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "User authentication failed",
                        StatusCode = StatusCodes.Status401Unauthorized
                    });
                }

                _logger.LogInformation("User {UserId} selecting Individual investor type", userId);
                var response = await _investorService.SelectInvestorTypeIndividualAsync(userId, request);

                if (response.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error selecting Individual investor type");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Select Joint investor type and create profile
        /// </summary>
        /// <param name="request">Joint investor type selection request</param>
        /// <returns>Created investor profile</returns>
        [HttpPost("select-type/joint")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<InvestorProfileResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SelectJointType([FromBody] SelectInvestorTypeJointRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for SelectJointType");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                var userId = GetUserIdFromClaims();
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID not found in claims");
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "User authentication failed",
                        StatusCode = StatusCodes.Status401Unauthorized
                    });
                }

                _logger.LogInformation("User {UserId} selecting Joint investor type", userId);
                var response = await _investorService.SelectInvestorTypeJointAsync(userId, request);

                if (response.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error selecting Joint investor type");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Select IRA investor type and create profile
        /// </summary>
        /// <param name="request">IRA investor type selection request</param>
        /// <returns>Created investor profile</returns>
        [HttpPost("select-type/ira")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<InvestorProfileResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SelectIRAType([FromBody] SelectInvestorTypeIRARequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for SelectIRAType");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                var userId = GetUserIdFromClaims();
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID not found in claims");
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "User authentication failed",
                        StatusCode = StatusCodes.Status401Unauthorized
                    });
                }

                _logger.LogInformation("User {UserId} selecting IRA investor type", userId);
                var response = await _investorService.SelectInvestorTypeIRAAsync(userId, request);

                if (response.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error selecting IRA investor type");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Select Trust investor type and create profile
        /// </summary>
        /// <param name="request">Trust investor type selection request</param>
        /// <returns>Created investor profile</returns>
        [HttpPost("select-type/trust")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<InvestorProfileResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SelectTrustType([FromBody] SelectInvestorTypeTrustRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for SelectTrustType");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                var userId = GetUserIdFromClaims();
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID not found in claims");
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "User authentication failed",
                        StatusCode = StatusCodes.Status401Unauthorized
                    });
                }

                _logger.LogInformation("User {UserId} selecting Trust investor type", userId);
                var response = await _investorService.SelectInvestorTypeTrustAsync(userId, request);

                if (response.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error selecting Trust investor type");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Select Entity investor type and create profile
        /// </summary>
        /// <param name="request">Entity investor type selection request</param>
        /// <returns>Created investor profile</returns>
        [HttpPost("select-type/entity")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<InvestorProfileResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SelectEntityType([FromBody] SelectInvestorTypeEntityRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for SelectEntityType");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                var userId = GetUserIdFromClaims();
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID not found in claims");
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "User authentication failed",
                        StatusCode = StatusCodes.Status401Unauthorized
                    });
                }

                _logger.LogInformation("User {UserId} selecting Entity investor type", userId);
                var response = await _investorService.SelectInvestorTypeEntityAsync(userId, request);

                if (response.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error selecting Entity investor type");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Get current authenticated user's investor profile
        /// </summary>
        /// <returns>Investor profile</returns>
        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<InvestorProfileResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = GetUserIdFromClaims();
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID not found in claims");
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "User authentication failed",
                        StatusCode = StatusCodes.Status401Unauthorized
                    });
                }

                _logger.LogInformation("Retrieving profile for user {UserId}", userId);
                var response = await _investorService.GetInvestorProfileAsync(userId);

                if (response.Success)
                {
                    return Ok(response);
                }

                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving investor profile");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Get investor profile by ID (Portal access only)
        /// </summary>
        /// <param name="id">Investor profile ID</param>
        /// <returns>Investor profile</returns>
        [HttpGet("profile/{id}")]
        [Authorize(Roles = "Advisor,OperationsTeam")]
        [ProducesResponseType(typeof(ApiResponse<InvestorProfileResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetProfileById(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving profile for investor ID {InvestorId}", id);
                var response = await _investorService.GetInvestorProfileByIdAsync(id);

                if (response.Success)
                {
                    return Ok(response);
                }

                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving investor profile by ID {InvestorId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Update investor accreditation status (Portal access only)
        /// </summary>
        /// <param name="id">Investor profile ID</param>
        /// <param name="request">Accreditation update request</param>
        /// <returns>Success status</returns>
        [HttpPut("profile/{id}/accreditation")]
        [Authorize(Roles = "Advisor,OperationsTeam")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateAccreditation(int id, [FromBody] UpdateAccreditationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for UpdateAccreditation");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate that AccreditationType is provided when IsAccredited is true
                if (request.IsAccredited && !request.AccreditationType.HasValue)
                {
                    _logger.LogWarning("AccreditationType is required when IsAccredited is true");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "AccreditationType is required when IsAccredited is true",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                _logger.LogInformation("Updating accreditation status for investor ID {InvestorId}", id);
                var response = await _investorService.UpdateAccreditationAsync(id, request.IsAccredited, request.AccreditationType);

                if (response.Success)
                {
                    return Ok(response);
                }

                // Check if it's a not found or bad request scenario
                if (response.StatusCode == StatusCodes.Status404NotFound)
                {
                    return NotFound(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating accreditation status for investor ID {InvestorId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Helper method to extract user ID from JWT claims
        /// </summary>
        /// <returns>User ID from claims or null if not found</returns>
        private string? GetUserIdFromClaims()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}