using TWS.Core.DTOs.Request.User;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.User;

namespace TWS.Core.Interfaces.IServices
{
    /// <summary>
    /// Service interface for User Profile management
    /// Handles business logic for user profile operations
    /// </summary>
    public interface IUserProfileService
    {
        /// <summary>
        /// Gets the current user's profile
        /// </summary>
        /// <param name="userId">User ID from JWT token</param>
        /// <returns>User profile details</returns>
        Task<ApiResponse<UserProfileResponse>> GetCurrentUserAsync(string userId);

        /// <summary>
        /// Updates the current user's profile
        /// </summary>
        /// <param name="userId">User ID from JWT token</param>
        /// <param name="request">Profile update request</param>
        /// <returns>Updated user profile details</returns>
        Task<ApiResponse<UserProfileResponse>> UpdateProfileAsync(string userId, UpdateUserProfileRequest request);

        /// <summary>
        /// Changes the current user's password
        /// </summary>
        /// <param name="userId">User ID from JWT token</param>
        /// <param name="request">Password change request</param>
        /// <returns>Success status</returns>
        Task<ApiResponse<bool>> ChangePasswordAsync(string userId, ChangePasswordRequest request);
    }
}