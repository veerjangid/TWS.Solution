namespace TWS.Core.DTOs.Response.Accreditation
{
    /// <summary>
    /// Response DTO for accreditation document information
    /// </summary>
    public class AccreditationDocumentResponse
    {
        /// <summary>
        /// Document ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Type of document (e.g., "Tax Return", "Pay Stub", "Bank Statement", "CPA Letter", "License")
        /// </summary>
        public string DocumentType { get; set; } = string.Empty;

        /// <summary>
        /// Path to the document in storage
        /// </summary>
        public string DocumentPath { get; set; } = string.Empty;

        /// <summary>
        /// Date when document was uploaded
        /// </summary>
        public DateTime UploadDate { get; set; }
    }
}