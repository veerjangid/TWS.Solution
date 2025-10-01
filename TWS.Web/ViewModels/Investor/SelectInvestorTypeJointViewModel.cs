using System.ComponentModel.DataAnnotations;

namespace TWS.Web.ViewModels.Investor
{
    public class SelectInvestorTypeJointViewModel
    {
        [Required(ErrorMessage = "Joint investment flag is required")]
        [Display(Name = "Is this a joint investment?")]
        public bool IsJointInvestment { get; set; }

        [Required(ErrorMessage = "Joint account type is required")]
        [Display(Name = "Joint Account Type")]
        public int JointAccountType { get; set; }

        [Required(ErrorMessage = "Primary first name is required")]
        [Display(Name = "Primary Account Holder First Name")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        public string PrimaryFirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Primary last name is required")]
        [Display(Name = "Primary Account Holder Last Name")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        public string PrimaryLastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Primary US citizen status is required")]
        [Display(Name = "Is Primary Account Holder a US Citizen?")]
        public bool PrimaryIsUSCitizen { get; set; }

        [Display(Name = "Secondary Account Holder First Name")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        public string? SecondaryFirstName { get; set; }

        [Display(Name = "Secondary Account Holder Last Name")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        public string? SecondaryLastName { get; set; }

        [Display(Name = "Is Secondary Account Holder a US Citizen?")]
        public bool? SecondaryIsUSCitizen { get; set; }

        [Required(ErrorMessage = "Accreditation status is required")]
        [Display(Name = "Are you an Accredited Investor?")]
        public bool IsAccredited { get; set; }

        [Display(Name = "Accreditation Type")]
        public int? AccreditationType { get; set; }
    }
}
