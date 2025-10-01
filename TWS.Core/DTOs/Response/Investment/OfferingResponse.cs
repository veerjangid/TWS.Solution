namespace TWS.Core.DTOs.Response.Investment
{
    /// <summary>
    /// Response DTO for Offering entity
    /// Contains offering details for investment opportunities
    /// Reference: APIDoc.md Section 12
    /// </summary>
    public class OfferingResponse
    {
        /// <summary>
        /// Offering ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the offering
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description of the offering
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Status of the offering (Raising, Closed, ComingSoon)
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Date when offering was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when offering was last modified
        /// </summary>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// Path to offering image/picture
        /// </summary>
        public string? ImagePath { get; set; }

        /// <summary>
        /// Type of offering: Private Placement 506(c), 1031 Exchange Investment, Universal Offering, Tax Strategy, Roth IRA Conversion
        /// </summary>
        public string OfferingType { get; set; } = string.Empty;

        /// <summary>
        /// Total value of investment
        /// </summary>
        public decimal? TotalValue { get; set; }

        /// <summary>
        /// Path to offering PDF document
        /// </summary>
        public string? PDFPath { get; set; }

        /// <summary>
        /// User ID of the advisor who created the offering
        /// </summary>
        public string? CreatedByUserId { get; set; }

        /// <summary>
        /// User ID of the last user who modified the offering
        /// </summary>
        public string? ModifiedByUserId { get; set; }
    }
}