using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.Document
{
    /// <summary>
    /// Request DTO for uploading a single document
    /// Reference: APIDoc.md Section 10.2
    /// Note: IFormFile is handled in the controller layer
    /// </summary>
    public class UploadDocumentRequest
    {
        /// <summary>
        /// Investor profile ID to associate the document with
        /// </summary>
        [Required(ErrorMessage = "InvestorProfileId is required")]
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Display name for the document (e.g., "Property Deed", "Insurance Policy")
        /// </summary>
        [Required(ErrorMessage = "DocumentName is required")]
        [MaxLength(200, ErrorMessage = "DocumentName cannot exceed 200 characters")]
        public string DocumentName { get; set; } = string.Empty;
    }
}