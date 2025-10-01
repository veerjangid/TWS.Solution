namespace TWS.Core.DTOs.Response.PrimaryInvestorInfo
{
    /// <summary>
    /// Response DTO for Source of Funds information
    /// </summary>
    public class SourceOfFundsResponse
    {
        /// <summary>
        /// Gets or sets the source of funds identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the primary investor info identifier
        /// </summary>
        public int PrimaryInvestorInfoId { get; set; }

        /// <summary>
        /// Gets or sets the source type (enum value)
        /// </summary>
        public int SourceType { get; set; }

        /// <summary>
        /// Gets or sets the source type name
        /// </summary>
        public string SourceTypeName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}