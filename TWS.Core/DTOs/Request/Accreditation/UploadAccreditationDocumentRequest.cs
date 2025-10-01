using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.Accreditation
{
    /// <summary>
    /// Request DTO for uploading accreditation document metadata
    /// Note: File upload is handled via IFormFile parameter in controller
    /// </summary>
    public class UploadAccreditationDocumentRequest
    {
        /// <summary>
        /// The investor accreditation ID
        /// </summary>
        [Required(ErrorMessage = "Investor Accreditation ID is required")]
        public int InvestorAccreditationId { get; set; }

        /// <summary>
        /// Type of document (e.g., "Tax Return", "Pay Stub", "Bank Statement", "CPA Letter", "License")
        /// </summary>
        [Required(ErrorMessage = "Document Type is required")]
        [MaxLength(100, ErrorMessage = "Document Type cannot exceed 100 characters")]
        public string DocumentType { get; set; } = string.Empty;
    }
}