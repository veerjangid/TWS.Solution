using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.PersonalFinancialStatement
{
    /// <summary>
    /// Request DTO for uploading a Personal Financial Statement document.
    /// Note: File is handled via IFormFile in controller.
    /// Reference: APIDoc.md Section 8
    /// </summary>
    public class UploadPFSRequest
    {
        /// <summary>
        /// ID of the investor profile to associate with the PFS document.
        /// </summary>
        [Required(ErrorMessage = "InvestorProfileId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "InvestorProfileId must be a positive integer")]
        public int InvestorProfileId { get; set; }
    }
}