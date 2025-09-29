namespace TWS.Core.Settings;

/// <summary>
/// Configuration settings for JWT token generation and validation
/// Reference: SecurityDesign.md
/// </summary>
public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiryMinutes { get; set; }
    public int RefreshTokenExpiryDays { get; set; }
}