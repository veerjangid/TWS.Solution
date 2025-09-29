using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TWS.Core.Constants;
using TWS.Core.DTOs.Request.Account;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Account;
using TWS.Core.Interfaces.IServices;

namespace TWS.API.Controllers
{
    /// <summary>
    /// Controller for managing account requests from potential investors
    /// Reference: APIDoc.md Section 2, BusinessRequirement.md Section 3.1
    /// </summary>
    [ApiController]
    [Route("api/request-account")]
    [Produces("application/json")]
    public class RequestAccountController : ControllerBase
    {
        private readonly IRequestAccountService _requestAccountService;
        private readonly ILogger<RequestAccountController> _logger;

        /// <summary>
        /// Constructor for RequestAccountController
        /// </summary>
        /// <param name="requestAccountService">Account request service</param>
        /// <param name="logger">Logger instance</param>
        public RequestAccountController(
            IRequestAccountService requestAccountService,
            ILogger<RequestAccountController> logger)
        {
            _requestAccountService = requestAccountService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new account request from a potential investor
        /// Public endpoint - no authentication required
        /// </summary>
        /// <param name="request">Account request details</param>
        /// <returns>Created account request</returns>
        /// <response code="201">Returns the newly created account request</response>
        /// <response code="400">If the request data is invalid</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<AccountRequestResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<AccountRequestResponse>>> CreateAccountRequest(
            [FromBody] CreateAccountRequestRequest request)
        {
            try
            {
                _logger.LogInformation("Creating new account request for email: {Email}", request.Email);

                // Validate model state
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                    _logger.LogWarning("Invalid model state for account request: {Errors}", errors);

                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        $"Validation failed: {errors}",
                        StatusCodes.Status400BadRequest));
                }

                var result = await _requestAccountService.CreateRequestAsync(request);

                if (!result.Success)
                {
                    _logger.LogWarning("Failed to create account request: {Message}", result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Account request created successfully with ID: {Id}", result.Data?.Id);

                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating account request for email: {Email}", request.Email);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse(
                        "An error occurred while creating the account request",
                        StatusCodes.Status500InternalServerError));
            }
        }

        /// <summary>
        /// Retrieves all account requests
        /// Restricted to Advisor and OperationsTeam roles
        /// </summary>
        /// <returns>List of all account requests</returns>
        /// <response code="200">Returns the list of account requests</response>
        /// <response code="401">If user is not authenticated</response>
        /// <response code="403">If user does not have required role</response>
        [HttpGet]
        [Authorize(Roles = $"{RoleConstants.Advisor},{RoleConstants.OperationsTeam}")]
        [ProducesResponseType(typeof(ApiResponse<List<AccountRequestResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<List<AccountRequestResponse>>>> GetAllAccountRequests()
        {
            try
            {
                _logger.LogInformation("Retrieving all account requests");

                var result = await _requestAccountService.GetAllRequestsAsync();

                if (!result.Success)
                {
                    _logger.LogWarning("Failed to retrieve account requests: {Message}", result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Retrieved {Count} account requests", result.Data?.Count ?? 0);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all account requests");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<List<AccountRequestResponse>>.ErrorResponse(
                        "An error occurred while retrieving account requests",
                        StatusCodes.Status500InternalServerError));
            }
        }

        /// <summary>
        /// Retrieves a specific account request by ID
        /// Restricted to Advisor and OperationsTeam roles
        /// </summary>
        /// <param name="id">Account request ID</param>
        /// <returns>Account request details</returns>
        /// <response code="200">Returns the account request</response>
        /// <response code="404">If account request not found</response>
        /// <response code="401">If user is not authenticated</response>
        /// <response code="403">If user does not have required role</response>
        [HttpGet("{id}")]
        [Authorize(Roles = $"{RoleConstants.Advisor},{RoleConstants.OperationsTeam}")]
        [ProducesResponseType(typeof(ApiResponse<AccountRequestResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<AccountRequestResponse>>> GetAccountRequestById(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving account request with ID: {Id}", id);

                var result = await _requestAccountService.GetRequestByIdAsync(id);

                if (!result.Success)
                {
                    _logger.LogWarning("Account request with ID {Id} not found", id);
                    return NotFound(result);
                }

                _logger.LogInformation("Account request with ID {Id} retrieved successfully", id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving account request with ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<AccountRequestResponse>.ErrorResponse(
                        "An error occurred while retrieving the account request",
                        StatusCodes.Status500InternalServerError));
            }
        }

        /// <summary>
        /// Processes an account request by marking it as processed
        /// Restricted to Advisor and OperationsTeam roles
        /// </summary>
        /// <param name="id">Account request ID</param>
        /// <param name="request">Processing details including optional notes</param>
        /// <returns>Updated account request</returns>
        /// <response code="200">Returns the updated account request</response>
        /// <response code="404">If account request not found</response>
        /// <response code="400">If the request data is invalid</response>
        /// <response code="401">If user is not authenticated</response>
        /// <response code="403">If user does not have required role</response>
        [HttpPut("{id}/process")]
        [Authorize(Roles = $"{RoleConstants.Advisor},{RoleConstants.OperationsTeam}")]
        [ProducesResponseType(typeof(ApiResponse<AccountRequestResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<AccountRequestResponse>>> ProcessAccountRequest(
            int id,
            [FromBody] ProcessAccountRequestRequest request)
        {
            try
            {
                _logger.LogInformation("Processing account request with ID: {Id}", id);

                // Validate model state
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                    _logger.LogWarning("Invalid model state for processing account request: {Errors}", errors);

                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        $"Validation failed: {errors}",
                        StatusCodes.Status400BadRequest));
                }

                // Extract userId from claims
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID not found in claims when processing account request");
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "User ID not found in authentication token",
                        StatusCodes.Status400BadRequest));
                }

                var result = await _requestAccountService.ProcessRequestAsync(id, request, userId);

                if (!result.Success)
                {
                    _logger.LogWarning("Failed to process account request with ID {Id}: {Message}", id, result.Message);

                    // Return appropriate status code based on message
                    if (result.StatusCode == StatusCodes.Status404NotFound)
                    {
                        return NotFound(result);
                    }

                    return BadRequest(result);
                }

                _logger.LogInformation("Account request with ID {Id} processed successfully by user {UserId}", id, userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing account request with ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<AccountRequestResponse>.ErrorResponse(
                        "An error occurred while processing the account request",
                        StatusCodes.Status500InternalServerError));
            }
        }

        /// <summary>
        /// Deletes an account request
        /// Restricted to OperationsTeam role only
        /// </summary>
        /// <param name="id">Account request ID</param>
        /// <returns>Success status</returns>
        /// <response code="200">If deletion was successful</response>
        /// <response code="404">If account request not found</response>
        /// <response code="401">If user is not authenticated</response>
        /// <response code="403">If user does not have required role</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleConstants.OperationsTeam)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAccountRequest(int id)
        {
            try
            {
                _logger.LogInformation("Deleting account request with ID: {Id}", id);

                var result = await _requestAccountService.DeleteRequestAsync(id);

                if (!result.Success)
                {
                    _logger.LogWarning("Failed to delete account request with ID {Id}: {Message}", id, result.Message);

                    // Return appropriate status code based on message
                    if (result.StatusCode == StatusCodes.Status404NotFound)
                    {
                        return NotFound(result);
                    }

                    return BadRequest(result);
                }

                _logger.LogInformation("Account request with ID {Id} deleted successfully", id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting account request with ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<bool>.ErrorResponse(
                        "An error occurred while deleting the account request",
                        StatusCodes.Status500InternalServerError));
            }
        }
    }
}
