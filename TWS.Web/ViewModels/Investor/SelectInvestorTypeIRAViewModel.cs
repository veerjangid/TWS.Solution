using System.ComponentModel.DataAnnotations;

namespace TWS.Web.ViewModels.Investor
{
    public class SelectInvestorTypeIRAViewModel
    {
        [Required(ErrorMessage = "IRA type is required")]
        [Display(Name = "IRA Account Type")]
        public int IRAType { get; set; }

        [Required(ErrorMessage = "Name of IRA is required")]
        [Display(Name = "Name of IRA")]
        [StringLength(200, ErrorMessage = "Name of IRA cannot exceed 200 characters")]
        public string NameOfIRA { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "Account Holder First Name")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Account Holder Last Name")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "US citizen status is required")]
        [Display(Name = "Is the account holder a US Citizen?")]
        public bool IsUSCitizen { get; set; }

        [Required(ErrorMessage = "Accreditation status is required")]
        [Display(Name = "Are you an Accredited Investor?")]
        public bool IsAccredited { get; set; }

        [Display(Name = "Accreditation Type")]
        public int? AccreditationType { get; set; }
    }
}
