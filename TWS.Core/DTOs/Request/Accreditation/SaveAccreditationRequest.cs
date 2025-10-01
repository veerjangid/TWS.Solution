using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.Accreditation
{
    /// <summary>
    /// Request DTO for saving investor accreditation information
    /// </summary>
    public class SaveAccreditationRequest
    {
        /// <summary>
        /// The investor profile ID
        /// </summary>
        [Required(ErrorMessage = "Investor Profile ID is required")]
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Accreditation type (1-4): NotAccredited, IncomeTest, NetWorthTest, LicensesAndCertifications
        /// </summary>
        [Required(ErrorMessage = "Accreditation Type is required")]
        [Range(1, 4, ErrorMessage = "Accreditation Type must be between 1 and 4")]
        public int AccreditationType { get; set; }

        /// <summary>
        /// License number (required if AccreditationType = LicensesAndCertifications)
        /// </summary>
        [MaxLength(100, ErrorMessage = "License Number cannot exceed 100 characters")]
        public string? LicenseNumber { get; set; }

        /// <summary>
        /// State where license is held (required if AccreditationType = LicensesAndCertifications)
        /// </summary>
        [MaxLength(50, ErrorMessage = "State License Held cannot exceed 50 characters")]
        public string? StateLicenseHeld { get; set; }
    }
}