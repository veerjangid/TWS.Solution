using System.ComponentModel.DataAnnotations;

namespace TWS.Web.ViewModels.Investor
{
    public class SelectInvestorTypeEntityViewModel
    {
        [Required(ErrorMessage = "Company name is required")]
        [Display(Name = "Company/Entity Name")]
        [StringLength(200, ErrorMessage = "Company name cannot exceed 200 characters")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "US company status is required")]
        [Display(Name = "Is this a US Company?")]
        public bool IsUSCompany { get; set; }

        [Required(ErrorMessage = "Entity type is required")]
        [Display(Name = "Entity Type")]
        public int EntityType { get; set; }

        [Required(ErrorMessage = "Accreditation status is required")]
        [Display(Name = "Is the entity an Accredited Investor?")]
        public bool IsAccredited { get; set; }

        [Display(Name = "Accreditation Type")]
        public int? AccreditationType { get; set; }
    }
}
