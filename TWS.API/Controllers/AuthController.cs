using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TWS.Core.Constants;
using TWS.Core.DTOs.Request.Auth;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Auth;
using TWS.Core.Interfaces.IServices;

namespace TWS.API.Controllers
{
    /// <summary>
    /// Controller for authentication operations including registration, login, and password management
    /// Reference: APIDoc.md Section 1, SecurityDesign.md
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Constructor for AuthController
        /// </summary>
        /// <param name="authService">Authentication service</param>
        /// <param name="logger">Logger instance</param>
        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user with specified role
        /// Public endpoint - no authentication required
        /// </summary>
        /// <param name="request">Registration details including email, password, name, and role</param>
        /// <returns>Registered user information</returns>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="400">If the request data is invalid or email already exists</response>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<RegisterResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<RegisterResponse>>> Register(
            [FromBody] RegisterRequest request)
        {
            try
            {
                _logger.LogInformation("Registration attempt for email: {Email}", request.Email);

                // Validate model state
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                    _logger.LogWarning("Invalid model state for registration: {Errors}", errors);

                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        $"Validation failed: {errors}",
                        StatusCodes.Status400BadRequest));
                }

                // Validate role against RoleConstants
                if (!RoleConstants.AllRoles.Contains(request.Role))
                {
                    _logger.LogWarning("Invalid role specified: {Role}", request.Role);

                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        $"Invalid role. Must be one of: {string.Join(", ", RoleConstants.AllRoles)}",
                        StatusCodes.Status400BadRequest));
                }

                var result = await _authService.RegisterAsync(request);

                if (!result.Success)
                {
                    _logger.LogWarning("Registration failed for email {Email}: {Message}", request.Email, result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("User {Email} registered successfully with role {Role}", request.Email, request.Role);

                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for email: {Email}", request.Email);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse(
                        "An error occurred during registration",
                        StatusCodes.Status500InternalServerError));
            }
        }

        /// <summary>
        /// Authenticates a user and generates JWT access token and refresh token
        /// Public endpoint - no authentication required
        /// </summary>
        /// <param name="request">Login credentials (email and password)</param>
        /// <returns>JWT tokens and user information</returns>
        /// <response code="200">Returns JWT tokens and user details</response>
        /// <response code="400">If the request data is invalid</response>
        /// <response code="401">If credentials are invalid</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Login(
            [FromBody] LoginRequest request)
        {
            try
            {
                _logger.LogInformation("Login attempt for email: {Email}", request.Email);

                // Validate model state
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                    _logger.LogWarning("Invalid model state for login: {Errors}", errors);

                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        $"Validation failed: {errors}",
                        StatusCodes.Status400BadRequest));
                }

                var result = await _authService.LoginAsync(request);

                if (!result.Success)
                {
                    _logger.LogWarning("Login failed for email {Email}: {Message}", request.Email, result.Message);

                    // Return 401 for authentication failures
                    if (result.StatusCode == StatusCodes.Status401Unauthorized)
                    {
                        return Unauthorized(result);
                    }

                    return BadRequest(result);
                }

                _logger.LogInformation("User {Email} logged in successfully", request.Email);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", request.Email);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse(
                        "An error occurred during login",
                        StatusCodes.Status500InternalServerError));
            }
        }

        /// <summary>
        /// Initiates password reset process by generating reset token
        /// In production, this would send an email with password reset link
        /// Public endpoint - no authentication required
        /// </summary>
        /// <param name="request">Email address for password reset</param>
        /// <returns>Success message</returns>
        /// <response code="200">Returns success message (even if email doesn't exist for security)</response>
        /// <response code="400">If the request data is invalid</response>
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<bool>>> ForgotPassword(
            [FromBody] ForgotPasswordRequest request)
        {
            try
            {
                _logger.LogInformation("Forgot password request for email: {Email}", request.Email);

                // Validate model state
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                    _logger.LogWarning("Invalid model state for forgot password: {Errors}", errors);

                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        $"Validation failed: {errors}",
                        StatusCodes.Status400BadRequest));
                }

                var result = await _authService.ForgotPasswordAsync(request);

                // Always return success to prevent email enumeration
                _logger.LogInformation("Forgot password request processed for email: {Email}", request.Email);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during forgot password for email: {Email}", request.Email);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse(
                        "An error occurred while processing your request",
                        StatusCodes.Status500InternalServerError));
            }
        }

        /// <summary>
        /// Resets user password using reset token
        /// Public endpoint - no authentication required (token acts as authentication)
        /// </summary>
        /// <param name="request">Password reset details including token and new password</param>
        /// <returns>Success status</returns>
        /// <response code="200">If password reset was successful</response>
        /// <response code="400">If the request data is invalid or token is invalid</response>
        [HttpPost("reset-password")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<bool>>> ResetPassword(
            [FromBody] ResetPasswordRequest request)
        {
            try
            {
                _logger.LogInformation("Password reset attempt for email: {Email}", request.Email);

                // Validate model state
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                    _logger.LogWarning("Invalid model state for password reset: {Errors}", errors);

                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        $"Validation failed: {errors}",
                        StatusCodes.Status400BadRequest));
                }

                var result = await _authService.ResetPasswordAsync(request);

                if (!result.Success)
                {
                    _logger.LogWarning("Password reset failed for email {Email}: {Message}", request.Email, result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Password reset successful for email: {Email}", request.Email);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password reset for email: {Email}", request.Email);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse(
                        "An error occurred during password reset",
                        StatusCodes.Status500InternalServerError));
            }
        }

        /// <summary>
        /// Generates new access token using valid refresh token
        /// Public endpoint - no authentication required (refresh token acts as authentication)
        /// </summary>
        /// <param name="request">Refresh token</param>
        /// <returns>New JWT access token and refresh token</returns>
        /// <response code="200">Returns new JWT tokens</response>
        /// <response code="400">If the request data is invalid</response>
        /// <response code="401">If refresh token is invalid or expired</response>
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> RefreshToken(
            [FromBody] RefreshTokenRequest request)
        {
            try
            {
                _logger.LogInformation("Token refresh attempt");

                // Validate model state
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                    _logger.LogWarning("Invalid model state for token refresh: {Errors}", errors);

                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        $"Validation failed: {errors}",
                        StatusCodes.Status400BadRequest));
                }

                var result = await _authService.RefreshTokenAsync(request);

                if (!result.Success)
                {
                    _logger.LogWarning("Token refresh failed: {Message}", result.Message);

                    // Return 401 for invalid/expired tokens
                    if (result.StatusCode == StatusCodes.Status401Unauthorized)
                    {
                        return Unauthorized(result);
                    }

                    return BadRequest(result);
                }

                _logger.LogInformation("Token refreshed successfully");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse(
                        "An error occurred during token refresh",
                        StatusCodes.Status500InternalServerError));
            }
        }

        /// <summary>
        /// Logs out user by revoking all active refresh tokens
        /// Requires authentication - user must be logged in
        /// </summary>
        /// <returns>Success status</returns>
        /// <response code="200">If logout was successful</response>
        /// <response code="401">If user is not authenticated</response>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<bool>>> Logout()
        {
            try
            {
                // Extract userId from claims
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID not found in claims during logout");
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "User ID not found in authentication token",
                        StatusCodes.Status400BadRequest));
                }

                _logger.LogInformation("Logout attempt for user: {UserId}", userId);

                var result = await _authService.LogoutAsync(userId);

                if (!result.Success)
                {
                    _logger.LogWarning("Logout failed for user {UserId}: {Message}", userId, result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("User {UserId} logged out successfully", userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse(
                        "An error occurred during logout",
                        StatusCodes.Status500InternalServerError));
            }
        }
    }
}