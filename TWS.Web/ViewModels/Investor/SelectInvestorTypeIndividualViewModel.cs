using System.ComponentModel.DataAnnotations;

namespace TWS.Web.ViewModels.Investor
{
    public class SelectInvestorTypeIndividualViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "US citizen status is required")]
        [Display(Name = "Are you a US Citizen?")]
        public bool IsUSCitizen { get; set; }

        [Required(ErrorMessage = "Accreditation status is required")]
        [Display(Name = "Are you an Accredited Investor?")]
        public bool IsAccredited { get; set; }

        [Display(Name = "Accreditation Type")]
        public int? AccreditationType { get; set; }
    }
}
