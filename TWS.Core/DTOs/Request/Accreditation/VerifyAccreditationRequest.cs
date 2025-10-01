using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.Accreditation
{
    /// <summary>
    /// Request DTO for verifying investor accreditation (Operations Team only)
    /// </summary>
    public class VerifyAccreditationRequest
    {
        /// <summary>
        /// Whether the accreditation is approved or rejected
        /// </summary>
        [Required(ErrorMessage = "Approval status is required")]
        public bool IsApproved { get; set; }

        /// <summary>
        /// Verification notes from Operations Team
        /// </summary>
        [MaxLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        public string? Notes { get; set; }
    }
}