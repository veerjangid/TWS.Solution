namespace TWS.Core.DTOs.Response.PersonalFinancialStatement
{
    /// <summary>
    /// Response DTO for Personal Financial Statement operations.
    /// Returns details of uploaded PFS document.
    /// Reference: APIDoc.md Section 8
    /// </summary>
    public class PFSResponse
    {
        /// <summary>
        /// Unique identifier for the PFS record.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID of the associated investor profile.
        /// </summary>
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// File path where the PDF document is stored.
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// Date and time when the document was initially uploaded.
        /// </summary>
        public DateTime UploadDate { get; set; }

        /// <summary>
        /// Date and time when the document was last modified/replaced.
        /// </summary>
        public DateTime? LastModifiedDate { get; set; }
    }
}