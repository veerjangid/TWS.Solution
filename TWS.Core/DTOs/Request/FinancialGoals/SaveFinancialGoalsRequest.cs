using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.FinancialGoals
{
    /// <summary>
    /// Request DTO for saving financial goals.
    /// Used for both creating new and updating existing financial goals.
    /// Reference: DatabaseSchema.md Table 27, APIDoc.md Section 9.1
    /// </summary>
    public class SaveFinancialGoalsRequest
    {
        /// <summary>
        /// Foreign key to InvestorProfile
        /// </summary>
        [Required(ErrorMessage = "InvestorProfileId is required.")]
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Liquidity needs (1=Low, 2=Medium, 3=High)
        /// </summary>
        [Required(ErrorMessage = "LiquidityNeeds is required.")]
        [Range(1, 3, ErrorMessage = "LiquidityNeeds must be between 1 and 3.")]
        public int LiquidityNeeds { get; set; }

        /// <summary>
        /// Investment timeline (1=0-5 years, 2=6-11 years, 3=12+ years)
        /// </summary>
        [Required(ErrorMessage = "InvestmentTimeline is required.")]
        [Range(1, 3, ErrorMessage = "InvestmentTimeline must be between 1 and 3.")]
        public int InvestmentTimeline { get; set; }

        /// <summary>
        /// Investment objective (1=Income, 2=Growth, 3=Income and Growth)
        /// </summary>
        [Required(ErrorMessage = "InvestmentObjective is required.")]
        [Range(1, 3, ErrorMessage = "InvestmentObjective must be between 1 and 3.")]
        public int InvestmentObjective { get; set; }

        /// <summary>
        /// Risk tolerance (1=Low, 2=Medium, 3=High)
        /// </summary>
        [Required(ErrorMessage = "RiskTolerance is required.")]
        [Range(1, 3, ErrorMessage = "RiskTolerance must be between 1 and 3.")]
        public int RiskTolerance { get; set; }

        /// <summary>
        /// Goal: Defer taxes
        /// </summary>
        [Required]
        public bool DeferTaxes { get; set; }

        /// <summary>
        /// Goal: Protect principal
        /// </summary>
        [Required]
        public bool ProtectPrincipal { get; set; }

        /// <summary>
        /// Goal: Grow principal
        /// </summary>
        [Required]
        public bool GrowPrincipal { get; set; }

        /// <summary>
        /// Goal: Consistent cash flow
        /// </summary>
        [Required]
        public bool ConsistentCashFlow { get; set; }

        /// <summary>
        /// Goal: Diversification
        /// </summary>
        [Required]
        public bool Diversification { get; set; }

        /// <summary>
        /// Goal: Retirement planning
        /// </summary>
        [Required]
        public bool Retirement { get; set; }

        /// <summary>
        /// Goal: Estate/Legacy planning
        /// </summary>
        [Required]
        public bool EstateLegacyPlanning { get; set; }

        /// <summary>
        /// Additional notes about financial goals
        /// </summary>
        [MaxLength(2000, ErrorMessage = "AdditionalNotes cannot exceed 2000 characters.")]
        public string? AdditionalNotes { get; set; }
    }
}