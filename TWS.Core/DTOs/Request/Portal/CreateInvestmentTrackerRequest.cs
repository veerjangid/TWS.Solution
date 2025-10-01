using System.ComponentModel.DataAnnotations;
using TWS.Core.Enums;

namespace TWS.Core.DTOs.Request.Portal
{
    /// <summary>
    /// Request DTO for creating a new investment tracker in Portal/CRM
    /// Reference: DatabaseSchema.md Table 32
    /// </summary>
    public class CreateInvestmentTrackerRequest
    {
        /// <summary>
        /// Foreign key to Offering entity
        /// </summary>
        [Required(ErrorMessage = "Offering ID is required")]
        public int OfferingId { get; set; }

        /// <summary>
        /// Foreign key to InvestorProfile entity
        /// </summary>
        [Required(ErrorMessage = "Investor Profile ID is required")]
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Investment status (13 status values)
        /// </summary>
        [Required(ErrorMessage = "Status is required")]
        public InvestmentStatus Status { get; set; }

        /// <summary>
        /// Lead owner licensed representative name
        /// </summary>
        [Required(ErrorMessage = "Lead Owner Licensed Rep is required")]
        [MaxLength(200, ErrorMessage = "Lead Owner Licensed Rep cannot exceed 200 characters")]
        public string LeadOwnerLicensedRep { get; set; } = string.Empty;

        /// <summary>
        /// Relationship type with investor
        /// </summary>
        [Required(ErrorMessage = "Relationship is required")]
        [MaxLength(200, ErrorMessage = "Relationship cannot exceed 200 characters")]
        public string Relationship { get; set; } = string.Empty;

        /// <summary>
        /// Type of portal investment
        /// </summary>
        [Required(ErrorMessage = "Investment Type is required")]
        public PortalInvestmentType InvestmentType { get; set; }

        /// <summary>
        /// Client name
        /// </summary>
        [Required(ErrorMessage = "Client Name is required")]
        [MaxLength(200, ErrorMessage = "Client Name cannot exceed 200 characters")]
        public string ClientName { get; set; } = string.Empty;

        /// <summary>
        /// Names in which investment is held
        /// </summary>
        [Required(ErrorMessage = "Investment Held In Names Of is required")]
        [MaxLength(500, ErrorMessage = "Investment Held In Names Of cannot exceed 500 characters")]
        public string InvestmentHeldInNamesOf { get; set; } = string.Empty;

        /// <summary>
        /// Date when investment was closed (nullable)
        /// </summary>
        public DateTime? DateInvestmentClosed { get; set; }

        /// <summary>
        /// Original equity investment amount
        /// </summary>
        [Required(ErrorMessage = "Original Equity Investment Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Original Equity Investment Amount must be non-negative")]
        public decimal OriginalEquityInvestmentAmount { get; set; }

        /// <summary>
        /// Total TWS Assets Under Management
        /// </summary>
        [Required(ErrorMessage = "Total TWS AUM is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Total TWS AUM must be non-negative")]
        public decimal TotalTWSAUM { get; set; }

        /// <summary>
        /// Representative commission amount
        /// </summary>
        [Required(ErrorMessage = "Rep Commission Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Rep Commission Amount must be non-negative")]
        public decimal RepCommissionAmount { get; set; }

        /// <summary>
        /// TWS Revenue
        /// </summary>
        [Required(ErrorMessage = "TWS Revenue is required")]
        [Range(0, double.MaxValue, ErrorMessage = "TWS Revenue must be non-negative")]
        public decimal TWSRevenue { get; set; }

        /// <summary>
        /// DST Revenue
        /// </summary>
        [Required(ErrorMessage = "DST Revenue is required")]
        [Range(0, double.MaxValue, ErrorMessage = "DST Revenue must be non-negative")]
        public decimal DSTRevenue { get; set; }

        /// <summary>
        /// Alternative Investment Revenue
        /// </summary>
        [Required(ErrorMessage = "Alt Revenue is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Alt Revenue must be non-negative")]
        public decimal AltRevenue { get; set; }

        /// <summary>
        /// Tax Strategy Revenue
        /// </summary>
        [Required(ErrorMessage = "Tax Strategy Revenue is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Tax Strategy Revenue must be non-negative")]
        public decimal TaxStrategyRevenue { get; set; }

        /// <summary>
        /// Oil and Gas Revenue
        /// </summary>
        [Required(ErrorMessage = "Oil And Gas Revenue is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Oil And Gas Revenue must be non-negative")]
        public decimal OilAndGasRevenue { get; set; }

        /// <summary>
        /// Initial vs Recurring Revenue classification
        /// </summary>
        [Required(ErrorMessage = "Initial Vs Recurring Revenue is required")]
        [MaxLength(100, ErrorMessage = "Initial Vs Recurring Revenue cannot exceed 100 characters")]
        public string InitialVsRecurringRevenue { get; set; } = string.Empty;

        /// <summary>
        /// Marketing source for this investment
        /// </summary>
        [Required(ErrorMessage = "Marketing Source is required")]
        [MaxLength(200, ErrorMessage = "Marketing Source cannot exceed 200 characters")]
        public string MarketingSource { get; set; } = string.Empty;

        /// <summary>
        /// Name of referrer (nullable)
        /// </summary>
        [MaxLength(200, ErrorMessage = "Referred By cannot exceed 200 characters")]
        public string? ReferredBy { get; set; }

        /// <summary>
        /// Additional notes (nullable)
        /// </summary>
        [MaxLength(2000, ErrorMessage = "Notes cannot exceed 2000 characters")]
        public string? Notes { get; set; }

        /// <summary>
        /// Alternative Investment AUM
        /// </summary>
        [Required(ErrorMessage = "Alt AUM is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Alt AUM must be non-negative")]
        public decimal AltAUM { get; set; }

        /// <summary>
        /// DST AUM
        /// </summary>
        [Required(ErrorMessage = "DST AUM is required")]
        [Range(0, double.MaxValue, ErrorMessage = "DST AUM must be non-negative")]
        public decimal DSTAUM { get; set; }

        /// <summary>
        /// Oil and Gas AUM
        /// </summary>
        [Required(ErrorMessage = "OG AUM is required")]
        [Range(0, double.MaxValue, ErrorMessage = "OG AUM must be non-negative")]
        public decimal OGAUM { get; set; }

        /// <summary>
        /// Tax Strategy AUM
        /// </summary>
        [Required(ErrorMessage = "Tax Strategy AUM is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Tax Strategy AUM must be non-negative")]
        public decimal TaxStrategyAUM { get; set; }
    }
}