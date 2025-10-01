namespace TWS.Core.DTOs.Response.Investment
{
    /// <summary>
    /// Response DTO for OfferingDocument entity
    /// Contains document details attached to an offering
    /// Reference: APIDoc.md Section 12
    /// </summary>
    public class OfferingDocumentResponse
    {
        /// <summary>
        /// Document ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Offering ID this document belongs to
        /// </summary>
        public int OfferingId { get; set; }

        /// <summary>
        /// Name of the document
        /// </summary>
        public string DocumentName { get; set; } = string.Empty;

        /// <summary>
        /// Path to the document file
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// Date when document was uploaded
        /// </summary>
        public DateTime UploadDate { get; set; }
    }
}