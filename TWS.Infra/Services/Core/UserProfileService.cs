using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TWS.Core.DTOs.Request.User;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.User;
using TWS.Core.Interfaces.IServices;
using TWS.Data.Entities.Identity;

namespace TWS.Infra.Services.Core
{
    /// <summary>
    /// Service implementation for User Profile management
    /// Handles business logic for user profile operations
    /// </summary>
    public class UserProfileService : IUserProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserProfileService> _logger;

        public UserProfileService(
            UserManager<ApplicationUser> userManager,
            ILogger<UserProfileService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Gets the current user's profile
        /// </summary>
        public async Task<ApiResponse<UserProfileResponse>> GetCurrentUserAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Retrieving profile for user {UserId}", userId);

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    _logger.LogWarning("User {UserId} not found", userId);
                    return ApiResponse<UserProfileResponse>.ErrorResponse("User not found", 404);
                }

                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "Investor";

                var response = new UserProfileResponse
                {
                    UserId = user.Id,
                    Email = user.Email ?? string.Empty,
                    FullName = user.FullName,
                    Role = role
                };

                _logger.LogInformation("Retrieved profile for user {UserId}", userId);
                return ApiResponse<UserProfileResponse>.SuccessResponse(response, "Profile retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving profile for user {UserId}", userId);
                return ApiResponse<UserProfileResponse>.ErrorResponse("Failed to retrieve profile", 500);
            }
        }

        /// <summary>
        /// Updates the current user's profile
        /// </summary>
        public async Task<ApiResponse<UserProfileResponse>> UpdateProfileAsync(string userId, UpdateUserProfileRequest request)
        {
            try
            {
                _logger.LogInformation("Updating profile for user {UserId}", userId);

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    _logger.LogWarning("User {UserId} not found", userId);
                    return ApiResponse<UserProfileResponse>.ErrorResponse("User not found", 404);
                }

                user.Email = request.Email;
                user.UserName = request.Email;
                user.FullName = $"{request.FirstName} {request.LastName}";
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Failed to update profile for user {UserId}: {Errors}", userId, errors);
                    return ApiResponse<UserProfileResponse>.ErrorResponse($"Failed to update profile: {errors}", 400);
                }

                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "Investor";

                var response = new UserProfileResponse
                {
                    UserId = user.Id,
                    Email = user.Email ?? string.Empty,
                    FullName = user.FullName,
                    Role = role
                };

                _logger.LogInformation("Updated profile for user {UserId}", userId);
                return ApiResponse<UserProfileResponse>.SuccessResponse(response, "Profile updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile for user {UserId}", userId);
                return ApiResponse<UserProfileResponse>.ErrorResponse("Failed to update profile", 500);
            }
        }

        /// <summary>
        /// Changes the current user's password
        /// </summary>
        public async Task<ApiResponse<bool>> ChangePasswordAsync(string userId, ChangePasswordRequest request)
        {
            try
            {
                _logger.LogInformation("Changing password for user {UserId}", userId);

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    _logger.LogWarning("User {UserId} not found", userId);
                    return ApiResponse<bool>.ErrorResponse("User not found", 404);
                }

                var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Failed to change password for user {UserId}: {Errors}", userId, errors);
                    return ApiResponse<bool>.ErrorResponse($"Failed to change password: {errors}", 400);
                }

                _logger.LogInformation("Changed password for user {UserId}", userId);
                return ApiResponse<bool>.SuccessResponse(true, "Password changed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for user {UserId}", userId);
                return ApiResponse<bool>.ErrorResponse("Failed to change password", 500);
            }
        }
    }
}