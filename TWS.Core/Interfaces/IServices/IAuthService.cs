using TWS.Core.DTOs.Request.Auth;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Auth;

namespace TWS.Core.Interfaces.IServices;

/// <summary>
/// Authentication service interface for user registration, login, and password management
/// Reference: APIDoc.md Section 1, SecurityDesign.md
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user and generates JWT tokens
    /// </summary>
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);

    /// <summary>
    /// Registers a new user with specified role
    /// </summary>
    Task<ApiResponse<RegisterResponse>> RegisterAsync(RegisterRequest request);

    /// <summary>
    /// Initiates password reset process by generating reset token
    /// </summary>
    Task<ApiResponse<bool>> ForgotPasswordAsync(ForgotPasswordRequest request);

    /// <summary>
    /// Resets user password using reset token
    /// </summary>
    Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequest request);

    /// <summary>
    /// Generates new access token using valid refresh token
    /// </summary>
    Task<ApiResponse<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request);

    /// <summary>
    /// Logs out user by revoking all active refresh tokens
    /// </summary>
    Task<ApiResponse<bool>> LogoutAsync(string userId);

    /// <summary>
    /// Revokes a specific refresh token
    /// </summary>
    Task<ApiResponse<bool>> RevokeTokenAsync(string token);
}