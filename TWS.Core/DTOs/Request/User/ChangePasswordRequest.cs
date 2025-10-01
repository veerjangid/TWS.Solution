using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.User
{
    /// <summary>
    /// Request DTO for changing user password
    /// Used by authenticated users to change their password
    /// </summary>
    public class ChangePasswordRequest
    {
        /// <summary>
        /// Current password
        /// </summary>
        [Required(ErrorMessage = "Current password is required")]
        public string OldPassword { get; set; } = string.Empty;

        /// <summary>
        /// New password (8-24 characters)
        /// </summary>
        [Required(ErrorMessage = "New password is required")]
        [StringLength(24, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 24 characters")]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// Confirm new password
        /// </summary>
        [Required(ErrorMessage = "Please confirm your new password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}