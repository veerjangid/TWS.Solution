using TWS.Core.Enums;

namespace TWS.Core.DTOs.Response.Portal
{
    /// <summary>
    /// Response DTO for investment tracker dashboard summary view
    /// Contains essential fields for Portal/CRM dashboard display
    /// Reference: DatabaseSchema.md Table 32
    /// </summary>
    public class DashboardItemResponse
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Offering name from navigation property
        /// </summary>
        public string OfferingName { get; set; } = string.Empty;

        /// <summary>
        /// Offering ID
        /// </summary>
        public int OfferingId { get; set; }

        /// <summary>
        /// Client name
        /// </summary>
        public string ClientName { get; set; } = string.Empty;

        /// <summary>
        /// Investor Profile ID
        /// </summary>
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Investment status
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Lead owner/advisor name
        /// </summary>
        public string LeadOwnerLicensedRep { get; set; } = string.Empty;

        /// <summary>
        /// Type of portal investment
        /// </summary>
        public string InvestmentType { get; set; } = string.Empty;

        /// <summary>
        /// Original equity investment amount
        /// </summary>
        public decimal OriginalEquityInvestmentAmount { get; set; }

        /// <summary>
        /// Total TWS AUM
        /// </summary>
        public decimal TotalTWSAUM { get; set; }

        /// <summary>
        /// Date when investment was closed (nullable)
        /// </summary>
        public DateTime? DateInvestmentClosed { get; set; }

        /// <summary>
        /// Timestamp when record was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Timestamp when record was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}