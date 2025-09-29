using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.Investor
{
    /// <summary>
    /// Request DTO for selecting Joint investor type
    /// </summary>
    public class SelectInvestorTypeJointRequest
    {
        [Required(ErrorMessage = "Joint investment flag is required")]
        public bool IsJointInvestment { get; set; }

        [Required(ErrorMessage = "Joint account type is required")]
        public int JointAccountType { get; set; }

        [Required(ErrorMessage = "Primary first name is required")]
        [StringLength(100, ErrorMessage = "Primary first name cannot exceed 100 characters")]
        public string PrimaryFirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Primary last name is required")]
        [StringLength(100, ErrorMessage = "Primary last name cannot exceed 100 characters")]
        public string PrimaryLastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Primary US citizen status is required")]
        public bool PrimaryIsUSCitizen { get; set; }

        [StringLength(100, ErrorMessage = "Secondary first name cannot exceed 100 characters")]
        public string? SecondaryFirstName { get; set; }

        [StringLength(100, ErrorMessage = "Secondary last name cannot exceed 100 characters")]
        public string? SecondaryLastName { get; set; }

        public bool? SecondaryIsUSCitizen { get; set; }

        [Required(ErrorMessage = "Accreditation status is required")]
        public bool IsAccredited { get; set; }

        public int? AccreditationType { get; set; }
    }
}