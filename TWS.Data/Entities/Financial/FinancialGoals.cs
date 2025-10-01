using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Core.Enums;
using TWS.Data.Entities.Core;

namespace TWS.Data.Entities.Financial
{
    /// <summary>
    /// Financial Goals entity representing investor's investment objectives and risk preferences
    /// Maps to FinancialGoals table (DatabaseSchema.md Table 27)
    /// One-to-one relationship with InvestorProfile
    /// </summary>
    public class FinancialGoals
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to InvestorProfile (one-to-one, unique)
        /// </summary>
        [Required]
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Investor's liquidity needs (Low, Medium, High)
        /// </summary>
        [Required]
        public LiquidityNeeds LiquidityNeeds { get; set; }

        /// <summary>
        /// Investment timeline (0-5 years, 6-11 years, 12+ years)
        /// </summary>
        [Required]
        public InvestmentTimeline InvestmentTimeline { get; set; }

        /// <summary>
        /// Investment objective (Income, Growth, Income and Growth)
        /// </summary>
        [Required]
        public InvestmentObjective InvestmentObjective { get; set; }

        /// <summary>
        /// Risk tolerance level (Low, Medium, High)
        /// </summary>
        [Required]
        public RiskTolerance RiskTolerance { get; set; }

        /// <summary>
        /// Goal: Defer taxes
        /// </summary>
        [Required]
        public bool DeferTaxes { get; set; } = false;

        /// <summary>
        /// Goal: Protect principal
        /// </summary>
        [Required]
        public bool ProtectPrincipal { get; set; } = false;

        /// <summary>
        /// Goal: Grow principal
        /// </summary>
        [Required]
        public bool GrowPrincipal { get; set; } = false;

        /// <summary>
        /// Goal: Consistent cash flow
        /// </summary>
        [Required]
        public bool ConsistentCashFlow { get; set; } = false;

        /// <summary>
        /// Goal: Diversification
        /// </summary>
        [Required]
        public bool Diversification { get; set; } = false;

        /// <summary>
        /// Goal: Retirement planning
        /// </summary>
        [Required]
        public bool Retirement { get; set; } = false;

        /// <summary>
        /// Goal: Estate/Legacy planning
        /// </summary>
        [Required]
        public bool EstateLegacyPlanning { get; set; } = false;

        /// <summary>
        /// Additional notes about financial goals
        /// </summary>
        [MaxLength(2000)]
        public string? AdditionalNotes { get; set; }

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
        /// Navigation property to InvestorProfile (one-to-one, required)
        /// </summary>
        [ForeignKey(nameof(InvestorProfileId))]
        public InvestorProfile InvestorProfile { get; set; } = null!;
    }
}