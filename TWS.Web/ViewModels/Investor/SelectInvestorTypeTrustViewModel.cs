using System.ComponentModel.DataAnnotations;

namespace TWS.Web.ViewModels.Investor
{
    public class SelectInvestorTypeTrustViewModel
    {
        [Required(ErrorMessage = "Trust name is required")]
        [Display(Name = "Trust Name")]
        [StringLength(200, ErrorMessage = "Trust name cannot exceed 200 characters")]
        public string TrustName { get; set; } = string.Empty;

        [Required(ErrorMessage = "US trust status is required")]
        [Display(Name = "Is this a US Trust?")]
        public bool IsUSTrust { get; set; }

        [Required(ErrorMessage = "Trust type is required")]
        [Display(Name = "Trust Type")]
        public int TrustType { get; set; }

        [Required(ErrorMessage = "Accreditation status is required")]
        [Display(Name = "Is the trust an Accredited Investor?")]
        public bool IsAccredited { get; set; }

        [Display(Name = "Accreditation Type")]
        public int? AccreditationType { get; set; }
    }
}
