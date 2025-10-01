using TWS.Core.Enums;

namespace TWS.Core.DTOs.Response.Portal
{
    /// <summary>
    /// Response DTO for investment tracker detailed view
    /// Contains all fields for Portal/CRM investment tracking
    /// Reference: DatabaseSchema.md Table 32
    /// </summary>
    public class InvestmentTrackerResponse
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to Offering entity
        /// </summary>
        public int OfferingId { get; set; }

        /// <summary>
        /// Offering name from navigation property
        /// </summary>
        public string OfferingName { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key to InvestorProfile entity
        /// </summary>
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Investment status
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Lead owner licensed representative name
        /// </summary>
        public string LeadOwnerLicensedRep { get; set; } = string.Empty;

        /// <summary>
        /// Relationship type with investor
        /// </summary>
        public string Relationship { get; set; } = string.Empty;

        /// <summary>
        /// Type of portal investment
        /// </summary>
        public string InvestmentType { get; set; } = string.Empty;

        /// <summary>
        /// Client name
        /// </summary>
        public string ClientName { get; set; } = string.Empty;

        /// <summary>
        /// Names in which investment is held
        /// </summary>
        public string InvestmentHeldInNamesOf { get; set; } = string.Empty;

        /// <summary>
        /// Date when investment was closed (nullable)
        /// </summary>
        public DateTime? DateInvestmentClosed { get; set; }

        /// <summary>
        /// Original equity investment amount
        /// </summary>
        public decimal OriginalEquityInvestmentAmount { get; set; }

        /// <summary>
        /// Total TWS Assets Under Management
        /// </summary>
        public decimal TotalTWSAUM { get; set; }

        /// <summary>
        /// Representative commission amount
        /// </summary>
        public decimal RepCommissionAmount { get; set; }

        /// <summary>
        /// TWS Revenue
        /// </summary>
        public decimal TWSRevenue { get; set; }

        /// <summary>
        /// DST Revenue
        /// </summary>
        public decimal DSTRevenue { get; set; }

        /// <summary>
        /// Alternative Investment Revenue
        /// </summary>
        public decimal AltRevenue { get; set; }

        /// <summary>
        /// Tax Strategy Revenue
        /// </summary>
        public decimal TaxStrategyRevenue { get; set; }

        /// <summary>
        /// Oil and Gas Revenue
        /// </summary>
        public decimal OilAndGasRevenue { get; set; }

        /// <summary>
        /// Initial vs Recurring Revenue classification
        /// </summary>
        public string InitialVsRecurringRevenue { get; set; } = string.Empty;

        /// <summary>
        /// Marketing source for this investment
        /// </summary>
        public string MarketingSource { get; set; } = string.Empty;

        /// <summary>
        /// Name of referrer (nullable)
        /// </summary>
        public string? ReferredBy { get; set; }

        /// <summary>
        /// Additional notes (nullable)
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Alternative Investment AUM
        /// </summary>
        public decimal AltAUM { get; set; }

        /// <summary>
        /// DST AUM
        /// </summary>
        public decimal DSTAUM { get; set; }

        /// <summary>
        /// Oil and Gas AUM
        /// </summary>
        public decimal OGAUM { get; set; }

        /// <summary>
        /// Tax Strategy AUM
        /// </summary>
        public decimal TaxStrategyAUM { get; set; }

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