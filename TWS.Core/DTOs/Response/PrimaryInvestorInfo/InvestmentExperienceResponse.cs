namespace TWS.Core.DTOs.Response.PrimaryInvestorInfo
{
    /// <summary>
    /// Response DTO for Investment Experience information
    /// </summary>
    public class InvestmentExperienceResponse
    {
        /// <summary>
        /// Gets or sets the investment experience identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the primary investor info identifier
        /// </summary>
        public int PrimaryInvestorInfoId { get; set; }

        /// <summary>
        /// Gets or sets the asset class (enum value)
        /// </summary>
        public int AssetClass { get; set; }

        /// <summary>
        /// Gets or sets the asset class name
        /// </summary>
        public string AssetClassName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the experience level (enum value)
        /// </summary>
        public int ExperienceLevel { get; set; }

        /// <summary>
        /// Gets or sets the experience level name
        /// </summary>
        public string ExperienceLevelName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description for "Other" asset class
        /// </summary>
        public string? OtherDescription { get; set; }

        /// <summary>
        /// Gets or sets the creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}