using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.Offering
{
    /// <summary>
    /// Request DTO for updating an existing offering
    /// Used by Advisors and Operations Team to modify investment opportunities
    /// Reference: APIDoc.md Section 12
    /// </summary>
    public class UpdateOfferingRequest
    {
        /// <summary>
        /// Name of the offering (investment opportunity)
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the offering
        /// </summary>
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Type of offering: Private Placement 506(c), 1031 Exchange Investment, Universal Offering, Tax Strategy, Roth IRA Conversion
        /// </summary>
        [Required(ErrorMessage = "Offering type is required")]
        [MaxLength(100, ErrorMessage = "Offering type cannot exceed 100 characters")]
        public string OfferingType { get; set; } = string.Empty;

        /// <summary>
        /// Total value of investment
        /// </summary>
        [Range(0, 999999999, ErrorMessage = "Total value must be between 0 and 999,999,999")]
        public decimal? TotalValue { get; set; }

        /// <summary>
        /// Status of the offering: 1=Raising, 2=Closed, 3=ComingSoon
        /// </summary>
        [Required(ErrorMessage = "Status is required")]
        [Range(1, 3, ErrorMessage = "Status must be 1 (Raising), 2 (Closed), or 3 (Coming Soon)")]
        public int Status { get; set; }
    }
}