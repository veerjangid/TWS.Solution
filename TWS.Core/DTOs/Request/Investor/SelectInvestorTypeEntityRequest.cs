using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.Investor
{
    /// <summary>
    /// Request DTO for selecting Entity investor type
    /// </summary>
    public class SelectInvestorTypeEntityRequest
    {
        [Required(ErrorMessage = "Company name is required")]
        [StringLength(200, ErrorMessage = "Company name cannot exceed 200 characters")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "US company status is required")]
        public bool IsUSCompany { get; set; }

        [Required(ErrorMessage = "Entity type is required")]
        public int EntityType { get; set; }

        [Required(ErrorMessage = "Accreditation status is required")]
        public bool IsAccredited { get; set; }

        public int? AccreditationType { get; set; }
    }
}