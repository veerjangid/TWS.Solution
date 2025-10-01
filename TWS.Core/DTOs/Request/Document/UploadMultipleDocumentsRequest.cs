using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.Document
{
    /// <summary>
    /// Request DTO for uploading multiple documents at once
    /// Reference: APIDoc.md Section 10.3
    /// Note: IFormFile list is handled in the controller layer
    /// </summary>
    public class UploadMultipleDocumentsRequest
    {
        /// <summary>
        /// Investor profile ID to associate the documents with
        /// </summary>
        [Required(ErrorMessage = "InvestorProfileId is required")]
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// List of document names corresponding to each uploaded file
        /// Must match the count of uploaded files
        /// </summary>
        [Required(ErrorMessage = "DocumentNames list is required")]
        [MinLength(1, ErrorMessage = "At least one document name is required")]
        public List<string> DocumentNames { get; set; } = new List<string>();
    }
}