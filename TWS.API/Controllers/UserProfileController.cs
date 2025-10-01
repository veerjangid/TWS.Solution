using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TWS.Core.DTOs.Request.User;
using TWS.Core.Interfaces.IServices;

namespace TWS.API.Controllers
{
    /// <summary>
    /// Manages user profile operations.
    /// Provides endpoints for authenticated users to view and update their profile.
    /// </summary>
    [ApiController]
    [Route("api/user-profile")]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly ILogger<UserProfileController> _logger;

        public UserProfileController(
            IUserProfileService userProfileService,
            ILogger<UserProfileController> logger)
        {
            _userProfileService = userProfileService;
            _logger = logger;
        }

        /// <summary>
        /// Gets the current authenticated user's profile.
        /// Extracts user ID from JWT token.
        /// </summary>
        /// <returns>User profile details</returns>
        /// <response code="200">Successfully retrieved profile</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="404">User not found</response>
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in token" });
                }

                _logger.LogInformation("Retrieving profile for user {UserId}", userId);

                var response = await _userProfileService.GetCurrentUserAsync(userId);

                if (!response.Success)
                {
                    return StatusCode(response.StatusCode, new { message = response.Message });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving current user profile");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving your profile" });
            }
        }

        /// <summary>
        /// Updates the current authenticated user's profile.
        /// Extracts user ID from JWT token.
        /// </summary>
        /// <param name="request">Profile update details</param>
        /// <returns>Updated user profile</returns>
        /// <response code="200">Successfully updated profile</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="404">User not found</response>
        [HttpPut("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateProfile([FromBody] UpdateUserProfileRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in token" });
                }

                _logger.LogInformation("Updating profile for user {UserId}", userId);

                var response = await _userProfileService.UpdateProfileAsync(userId, request);

                if (!response.Success)
                {
                    return StatusCode(response.StatusCode, new { message = response.Message });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user profile");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while updating your profile" });
            }
        }

        /// <summary>
        /// Changes the current authenticated user's password.
        /// Extracts user ID from JWT token.
        /// </summary>
        /// <param name="request">Password change details</param>
        /// <returns>Success confirmation</returns>
        /// <response code="200">Successfully changed password</response>
        /// <response code="400">Invalid request data or incorrect old password</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="404">User not found</response>
        [HttpPut("change-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in token" });
                }

                _logger.LogInformation("Changing password for user {UserId}", userId);

                var response = await _userProfileService.ChangePasswordAsync(userId, request);

                if (!response.Success)
                {
                    return StatusCode(response.StatusCode, new { message = response.Message });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while changing your password" });
            }
        }
    }
}