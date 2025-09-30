using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.GeneralInfo
{
    public class SaveEntityGeneralInfoRequest
    {
        [Required(ErrorMessage = "Investor Profile ID is required")]
        public int InvestorProfileId { get; set; }

        [Required(ErrorMessage = "Company Name is required")]
        [MaxLength(200, ErrorMessage = "Company Name cannot exceed 200 characters")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "IsUSCompany flag is required")]
        public bool IsUSCompany { get; set; }

        [Required(ErrorMessage = "Entity Type is required")]
        public int EntityType { get; set; }

        [Required(ErrorMessage = "Date of Formation is required")]
        public DateTime DateOfFormation { get; set; }

        [Required(ErrorMessage = "Purpose of Formation is required")]
        [MaxLength(1000, ErrorMessage = "Purpose of Formation cannot exceed 1000 characters")]
        public string PurposeOfFormation { get; set; } = string.Empty;

        [Required(ErrorMessage = "TIN/EIN is required")]
        [MaxLength(50, ErrorMessage = "TIN/EIN cannot exceed 50 characters")]
        public string TINEIN { get; set; } = string.Empty;

        [Required(ErrorMessage = "HasOperatingAgreement flag is required")]
        public bool HasOperatingAgreement { get; set; }
    }
}