namespace TWS.Core.DTOs.Response.Investment
{
    /// <summary>
    /// Response DTO for InvestorInvestment entity
    /// Contains basic investment details
    /// Reference: APIDoc.md Section 12
    /// </summary>
    public class InvestmentResponse
    {
        /// <summary>
        /// Investment ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Investor profile ID
        /// </summary>
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Offering ID
        /// </summary>
        public int OfferingId { get; set; }

        /// <summary>
        /// Name of the offering
        /// </summary>
        public string OfferingName { get; set; } = string.Empty;

        /// <summary>
        /// Date when investment was made
        /// </summary>
        public DateTime InvestmentDate { get; set; }

        /// <summary>
        /// Investment amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Investment status
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Notes about the investment
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Date when investment was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date when investment was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}