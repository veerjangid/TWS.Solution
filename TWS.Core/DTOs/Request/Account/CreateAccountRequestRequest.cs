using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.Account
{
    /// <summary>
    /// DTO for creating a new account request
    /// Used when a potential investor submits a request for account opening
    /// Reference: BusinessRequirement.md Section 3.1, FunctionalRequirement.md
    /// </summary>
    public class CreateAccountRequestRequest
    {
        /// <summary>
        /// First name of the requester
        /// </summary>
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of the requester
        /// </summary>
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the requester
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Phone number of the requester
        /// </summary>
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [MaxLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Optional message from the requester
        /// </summary>
        [MaxLength(1000, ErrorMessage = "Message cannot exceed 1000 characters")]
        public string? Message { get; set; }
    }
}