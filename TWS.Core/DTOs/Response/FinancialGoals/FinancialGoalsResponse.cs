namespace TWS.Core.DTOs.Response.FinancialGoals
{
    /// <summary>
    /// Response DTO for Financial Goals operations.
    /// Returns investor's investment objectives and risk preferences.
    /// Reference: APIDoc.md Section 9, DatabaseSchema.md Table 27
    /// </summary>
    public class FinancialGoalsResponse
    {
        /// <summary>
        /// Unique identifier for the Financial Goals record.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID of the associated investor profile.
        /// </summary>
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Liquidity needs (Low, Medium, High)
        /// </summary>
        public string LiquidityNeeds { get; set; } = string.Empty;

        /// <summary>
        /// Investment timeline (ZeroToFiveYears, SixToElevenYears, TwelvePlusYears)
        /// </summary>
        public string InvestmentTimeline { get; set; } = string.Empty;

        /// <summary>
        /// Investment objective (Income, Growth, IncomeAndGrowth)
        /// </summary>
        public string InvestmentObjective { get; set; } = string.Empty;

        /// <summary>
        /// Risk tolerance level (LowRisk, MediumRisk, HighRisk)
        /// </summary>
        public string RiskTolerance { get; set; } = string.Empty;

        /// <summary>
        /// Goal: Defer taxes
        /// </summary>
        public bool DeferTaxes { get; set; }

        /// <summary>
        /// Goal: Protect principal
        /// </summary>
        public bool ProtectPrincipal { get; set; }

        /// <summary>
        /// Goal: Grow principal
        /// </summary>
        public bool GrowPrincipal { get; set; }

        /// <summary>
        /// Goal: Consistent cash flow
        /// </summary>
        public bool ConsistentCashFlow { get; set; }

        /// <summary>
        /// Goal: Diversification
        /// </summary>
        public bool Diversification { get; set; }

        /// <summary>
        /// Goal: Retirement planning
        /// </summary>
        public bool Retirement { get; set; }

        /// <summary>
        /// Goal: Estate/Legacy planning
        /// </summary>
        public bool EstateLegacyPlanning { get; set; }

        /// <summary>
        /// Additional notes about financial goals
        /// </summary>
        public string? AdditionalNotes { get; set; }

        /// <summary>
        /// Date and time when the record was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date and time when the record was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}