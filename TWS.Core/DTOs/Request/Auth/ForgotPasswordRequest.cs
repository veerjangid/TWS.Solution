using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.Auth;

public class ForgotPasswordRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;
}