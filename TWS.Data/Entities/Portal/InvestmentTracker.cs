using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Core.Enums;

namespace TWS.Data.Entities.Portal
{
    /// <summary>
    /// InvestmentTracker entity for Portal/CRM module
    /// Maps to InvestmentTrackers table (DatabaseSchema.md Table 32)
    /// Tracks investments with complex financial metrics for Advisor and OperationsTeam
    /// Links to InvestorProfile and Offering entities
    /// </summary>
    public class InvestmentTracker
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to Offering entity
        /// </summary>
        [Required]
        public int OfferingId { get; set; }

        /// <summary>
        /// Foreign key to InvestorProfile entity
        /// </summary>
        [Required]
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Investment status (13 status values)
        /// </summary>
        [Required]
        public InvestmentStatus Status { get; set; }

        /// <summary>
        /// Lead owner licensed representative name
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string LeadOwnerLicensedRep { get; set; } = string.Empty;

        /// <summary>
        /// Relationship type with investor
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Relationship { get; set; } = string.Empty;

        /// <summary>
        /// Type of portal investment
        /// </summary>
        [Required]
        public PortalInvestmentType InvestmentType { get; set; }

        /// <summary>
        /// Client name
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string ClientName { get; set; } = string.Empty;

        /// <summary>
        /// Names in which investment is held
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string InvestmentHeldInNamesOf { get; set; } = string.Empty;

        /// <summary>
        /// Date when investment was closed (nullable)
        /// </summary>
        public DateTime? DateInvestmentClosed { get; set; }

        /// <summary>
        /// Original equity investment amount
        /// Precision (18,2)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalEquityInvestmentAmount { get; set; }

        /// <summary>
        /// Total TWS Assets Under Management
        /// Precision (18,2)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalTWSAUM { get; set; }

        /// <summary>
        /// Representative commission amount
        /// Precision (18,2)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RepCommissionAmount { get; set; }

        /// <summary>
        /// TWS Revenue
        /// Precision (18,2)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TWSRevenue { get; set; }

        /// <summary>
        /// DST Revenue
        /// Precision (18,2)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DSTRevenue { get; set; }

        /// <summary>
        /// Alternative Investment Revenue
        /// Precision (18,2)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AltRevenue { get; set; }

        /// <summary>
        /// Tax Strategy Revenue
        /// Precision (18,2)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxStrategyRevenue { get; set; }

        /// <summary>
        /// Oil and Gas Revenue
        /// Precision (18,2)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OilAndGasRevenue { get; set; }

        /// <summary>
        /// Initial vs Recurring Revenue classification
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string InitialVsRecurringRevenue { get; set; } = string.Empty;

        /// <summary>
        /// Marketing source for this investment
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string MarketingSource { get; set; } = string.Empty;

        /// <summary>
        /// Name of referrer (nullable)
        /// </summary>
        [MaxLength(200)]
        public string? ReferredBy { get; set; }

        /// <summary>
        /// Additional notes (nullable)
        /// </summary>
        [MaxLength(2000)]
        public string? Notes { get; set; }

        /// <summary>
        /// Alternative Investment AUM
        /// Precision (18,2)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AltAUM { get; set; }

        /// <summary>
        /// DST AUM
        /// Precision (18,2)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DSTAUM { get; set; }

        /// <summary>
        /// Oil and Gas AUM
        /// Precision (18,2)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OGAUM { get; set; }

        /// <summary>
        /// Tax Strategy AUM
        /// Precision (18,2)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxStrategyAUM { get; set; }

        /// <summary>
        /// Timestamp when record was created
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when record was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties

        /// <summary>
        /// Navigation property to Offering (many-to-one, required)
        /// </summary>
        [ForeignKey(nameof(OfferingId))]
        public Offering Offering { get; set; } = null!;

        /// <summary>
        /// Navigation property to InvestorProfile (many-to-one, required)
        /// </summary>
        [ForeignKey(nameof(InvestorProfileId))]
        public TWS.Data.Entities.Core.InvestorProfile InvestorProfile { get; set; } = null!;
    }
}