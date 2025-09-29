using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TWS.Core.Constants;
using TWS.Core.DTOs.Request.Auth;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Auth;
using TWS.Core.Interfaces.IServices;
using TWS.Data.Entities.Identity;

namespace TWS.Infra.Services.Core;

/// <summary>
/// Authentication service implementation for user registration, login, and password management
/// Reference: APIDoc.md Section 1, SecurityDesign.md
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ITokenService tokenService,
        IMapper mapper,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Authenticates a user and generates JWT tokens
    /// </summary>
    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Login attempt failed: User not found for email {Email}", request.Email);
                return ApiResponse<LoginResponse>.ErrorResponse("Invalid email or password", 401);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Login attempt failed: Invalid password for email {Email}", request.Email);
                return ApiResponse<LoginResponse>.ErrorResponse("Invalid email or password", 401);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? RoleConstants.Investor;

            var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email!, user.FullName, role);
            var refreshToken = _tokenService.GenerateRefreshToken();
            await _tokenService.SaveRefreshTokenAsync(user.Id, refreshToken);

            var response = new LoginResponse
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                UserId = user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                Role = role
            };

            _logger.LogInformation("User {Email} logged in successfully", request.Email);
            return ApiResponse<LoginResponse>.SuccessResponse(response, "Login successful");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email {Email}", request.Email);
            return ApiResponse<LoginResponse>.ErrorResponse("An error occurred during login", 500);
        }
    }

    /// <summary>
    /// Registers a new user with specified role
    /// </summary>
    public async Task<ApiResponse<RegisterResponse>> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Registration failed: Email {Email} already exists", request.Email);
                return ApiResponse<RegisterResponse>.ErrorResponse("Email already registered", 400);
            }

            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                _logger.LogWarning("Registration failed: Invalid role {Role}", request.Role);
                return ApiResponse<RegisterResponse>.ErrorResponse("Invalid role specified", 400);
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = $"{request.FirstName} {request.LastName}",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Registration failed for email {Email}: {Errors}", request.Email, errors);
                return ApiResponse<RegisterResponse>.ErrorResponse($"Registration failed: {errors}", 400);
            }

            await _userManager.AddToRoleAsync(user, request.Role);

            var response = new RegisterResponse
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = request.Role
            };

            _logger.LogInformation("User {Email} registered successfully with role {Role}", request.Email, request.Role);
            return ApiResponse<RegisterResponse>.SuccessResponse(response, "Registration successful", 201);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email {Email}", request.Email);
            return ApiResponse<RegisterResponse>.ErrorResponse("An error occurred during registration", 500);
        }
    }

    /// <summary>
    /// Initiates password reset process by generating reset token
    /// </summary>
    public async Task<ApiResponse<bool>> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Forgot password request for non-existent email {Email}", request.Email);
                return ApiResponse<bool>.SuccessResponse(true, "If the email exists, a password reset link has been sent");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // TODO: Send email with reset token
            // Email service integration will be implemented in a later phase

            _logger.LogInformation("Password reset token generated for user {Email}", request.Email);
            return ApiResponse<bool>.SuccessResponse(true, "Password reset link has been sent to your email");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during forgot password for email {Email}", request.Email);
            return ApiResponse<bool>.ErrorResponse("An error occurred while processing your request", 500);
        }
    }

    /// <summary>
    /// Resets user password using reset token
    /// </summary>
    public async Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Password reset failed: User not found for email {Email}", request.Email);
                return ApiResponse<bool>.ErrorResponse("Invalid password reset request", 400);
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Password reset failed for email {Email}: {Errors}", request.Email, errors);
                return ApiResponse<bool>.ErrorResponse($"Password reset failed: {errors}", 400);
            }

            _logger.LogInformation("Password reset successful for user {Email}", request.Email);
            return ApiResponse<bool>.SuccessResponse(true, "Password reset successful");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset for email {Email}", request.Email);
            return ApiResponse<bool>.ErrorResponse("An error occurred during password reset", 500);
        }
    }

    /// <summary>
    /// Generates new access token using valid refresh token
    /// </summary>
    public async Task<ApiResponse<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        try
        {
            var userId = await _tokenService.ValidateRefreshTokenAsync(request.RefreshToken);
            if (userId == null)
            {
                _logger.LogWarning("Refresh token validation failed");
                return ApiResponse<LoginResponse>.ErrorResponse("Invalid or expired refresh token", 401);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Refresh token validation failed: User not found for ID {UserId}", userId);
                return ApiResponse<LoginResponse>.ErrorResponse("Invalid refresh token", 401);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? RoleConstants.Investor;

            var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email!, user.FullName, role);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            await _tokenService.RevokeRefreshTokenAsync(request.RefreshToken);
            await _tokenService.SaveRefreshTokenAsync(user.Id, newRefreshToken);

            var response = new LoginResponse
            {
                Token = accessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                UserId = user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                Role = role
            };

            _logger.LogInformation("Token refreshed successfully for user {UserId}", userId);
            return ApiResponse<LoginResponse>.SuccessResponse(response, "Token refreshed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return ApiResponse<LoginResponse>.ErrorResponse("An error occurred during token refresh", 500);
        }
    }

    /// <summary>
    /// Logs out user by revoking all active refresh tokens
    /// </summary>
    public async Task<ApiResponse<bool>> LogoutAsync(string userId)
    {
        try
        {
            await _tokenService.RevokeAllUserTokensAsync(userId);

            _logger.LogInformation("User {UserId} logged out successfully", userId);
            return ApiResponse<bool>.SuccessResponse(true, "Logout successful");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout for user {UserId}", userId);
            return ApiResponse<bool>.ErrorResponse("An error occurred during logout", 500);
        }
    }

    /// <summary>
    /// Revokes a specific refresh token
    /// </summary>
    public async Task<ApiResponse<bool>> RevokeTokenAsync(string token)
    {
        try
        {
            await _tokenService.RevokeRefreshTokenAsync(token);

            _logger.LogInformation("Token revoked successfully");
            return ApiResponse<bool>.SuccessResponse(true, "Token revoked successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token revocation");
            return ApiResponse<bool>.ErrorResponse("An error occurred during token revocation", 500);
        }
    }
}