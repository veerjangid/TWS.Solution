namespace TWS.Core.DTOs.Response.Document
{
    /// <summary>
    /// Response DTO for document operations
    /// Returns document information without sensitive data
    /// Reference: APIDoc.md Section 10
    /// </summary>
    public class DocumentResponse
    {
        /// <summary>
        /// Document ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Investor profile ID
        /// </summary>
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Display name of the document
        /// </summary>
        public string DocumentName { get; set; } = string.Empty;

        /// <summary>
        /// File path where document is stored
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// Original file name
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// File size in bytes
        /// </summary>
        public long? FileSize { get; set; }

        /// <summary>
        /// MIME type of the file
        /// </summary>
        public string? ContentType { get; set; }

        /// <summary>
        /// Date and time when document was uploaded
        /// </summary>
        public DateTime UploadDate { get; set; }
    }
}