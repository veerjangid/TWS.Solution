using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.Investor
{
    /// <summary>
    /// Request DTO for selecting Trust investor type
    /// </summary>
    public class SelectInvestorTypeTrustRequest
    {
        [Required(ErrorMessage = "Trust name is required")]
        [StringLength(200, ErrorMessage = "Trust name cannot exceed 200 characters")]
        public string TrustName { get; set; } = string.Empty;

        [Required(ErrorMessage = "US trust status is required")]
        public bool IsUSTrust { get; set; }

        [Required(ErrorMessage = "Trust type is required")]
        public int TrustType { get; set; }

        [Required(ErrorMessage = "Accreditation status is required")]
        public bool IsAccredited { get; set; }

        public int? AccreditationType { get; set; }
    }
}