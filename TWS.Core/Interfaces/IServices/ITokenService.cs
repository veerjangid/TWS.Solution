using System.Security.Claims;

namespace TWS.Core.Interfaces.IServices;

/// <summary>
/// Token service interface for JWT and refresh token management
/// Reference: SecurityDesign.md
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates JWT access token for authenticated user
    /// </summary>
    string GenerateAccessToken(string userId, string email, string fullName, string role);

    /// <summary>
    /// Generates secure random refresh token
    /// </summary>
    string GenerateRefreshToken();

    /// <summary>
    /// Saves refresh token to database and returns the token string
    /// </summary>
    Task<string> SaveRefreshTokenAsync(string userId, string token);

    /// <summary>
    /// Validates refresh token and returns user ID if valid
    /// </summary>
    Task<string?> ValidateRefreshTokenAsync(string token);

    /// <summary>
    /// Revokes a specific refresh token
    /// </summary>
    Task RevokeRefreshTokenAsync(string token);

    /// <summary>
    /// Revokes all refresh tokens for a user
    /// </summary>
    Task RevokeAllUserTokensAsync(string userId);

    /// <summary>
    /// Extracts claims principal from expired JWT token
    /// </summary>
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}