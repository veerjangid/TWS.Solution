namespace TWS.Core.DTOs.Response.PrimaryInvestorInfo
{
    /// <summary>
    /// Response DTO for Tax Rate information
    /// </summary>
    public class TaxRateResponse
    {
        /// <summary>
        /// Gets or sets the tax rate identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the primary investor info identifier
        /// </summary>
        public int PrimaryInvestorInfoId { get; set; }

        /// <summary>
        /// Gets or sets the tax rate range (enum value)
        /// </summary>
        public int TaxRateRange { get; set; }

        /// <summary>
        /// Gets or sets the tax rate range name
        /// </summary>
        public string TaxRateRangeName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}