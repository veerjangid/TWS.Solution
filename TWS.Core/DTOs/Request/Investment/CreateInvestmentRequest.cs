using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.Investment
{
    /// <summary>
    /// Request DTO for creating a new investment
    /// Used when investor invests in an offering
    /// Reference: APIDoc.md Section 12
    /// </summary>
    public class CreateInvestmentRequest
    {
        /// <summary>
        /// Investor profile ID
        /// </summary>
        [Required(ErrorMessage = "Investor profile ID is required")]
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Offering ID to invest in
        /// </summary>
        [Required(ErrorMessage = "Offering ID is required")]
        public int OfferingId { get; set; }

        /// <summary>
        /// Investment amount
        /// </summary>
        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Optional notes about the investment
        /// </summary>
        [MaxLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        public string? Notes { get; set; }
    }
}