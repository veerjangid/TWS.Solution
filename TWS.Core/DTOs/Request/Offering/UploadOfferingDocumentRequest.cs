using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.Offering
{
    /// <summary>
    /// Request DTO for uploading documents to an offering
    /// Used by Advisors and Operations Team to add additional documents
    /// Reference: APIDoc.md Section 12
    /// </summary>
    public class UploadOfferingDocumentRequest
    {
        /// <summary>
        /// Offering ID to attach document to
        /// </summary>
        [Required(ErrorMessage = "Offering ID is required")]
        public int OfferingId { get; set; }

        /// <summary>
        /// Name of the document
        /// </summary>
        [Required(ErrorMessage = "Document name is required")]
        [MaxLength(200, ErrorMessage = "Document name cannot exceed 200 characters")]
        public string DocumentName { get; set; } = string.Empty;
    }
}