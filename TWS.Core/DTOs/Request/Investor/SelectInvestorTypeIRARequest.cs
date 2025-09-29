using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.Investor
{
    /// <summary>
    /// Request DTO for selecting IRA investor type
    /// </summary>
    public class SelectInvestorTypeIRARequest
    {
        [Required(ErrorMessage = "IRA type is required")]
        public int IRAType { get; set; }

        [Required(ErrorMessage = "Name of IRA is required")]
        [StringLength(200, ErrorMessage = "Name of IRA cannot exceed 200 characters")]
        public string NameOfIRA { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "US citizen status is required")]
        public bool IsUSCitizen { get; set; }

        [Required(ErrorMessage = "Accreditation status is required")]
        public bool IsAccredited { get; set; }

        public int? AccreditationType { get; set; }
    }
}