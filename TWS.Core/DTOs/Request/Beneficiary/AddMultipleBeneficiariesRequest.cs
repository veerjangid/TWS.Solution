using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.Beneficiary
{
    public class AddMultipleBeneficiariesRequest
    {
        [Required(ErrorMessage = "InvestorProfileId is required")]
        public int InvestorProfileId { get; set; }

        [Required(ErrorMessage = "Beneficiaries list is required")]
        [MinLength(1, ErrorMessage = "At least one beneficiary is required")]
        public List<BeneficiaryItem> Beneficiaries { get; set; }
    }

    public class BeneficiaryItem
    {
        [Required(ErrorMessage = "BeneficiaryType is required")]
        [Range(1, 2, ErrorMessage = "BeneficiaryType must be 1 (Primary) or 2 (Contingent)")]
        public int BeneficiaryType { get; set; }

        [Required(ErrorMessage = "FirstMiddleLastName is required")]
        [MaxLength(200, ErrorMessage = "FirstMiddleLastName cannot exceed 200 characters")]
        public string FirstMiddleLastName { get; set; }

        [Required(ErrorMessage = "SocialSecurityNumber is required")]
        [MaxLength(11, ErrorMessage = "SocialSecurityNumber cannot exceed 11 characters")]
        [RegularExpression(@"^\d{3}-\d{2}-\d{4}$", ErrorMessage = "SocialSecurityNumber must be in format XXX-XX-XXXX")]
        public string SocialSecurityNumber { get; set; }

        [Required(ErrorMessage = "DateOfBirth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [MaxLength(20, ErrorMessage = "Phone cannot exceed 20 characters")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "RelationshipToOwner is required")]
        [MaxLength(100, ErrorMessage = "RelationshipToOwner cannot exceed 100 characters")]
        public string RelationshipToOwner { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [MaxLength(100, ErrorMessage = "City cannot exceed 100 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [MaxLength(50, ErrorMessage = "State cannot exceed 50 characters")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zip is required")]
        [MaxLength(10, ErrorMessage = "Zip cannot exceed 10 characters")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "PercentageOfBenefit is required")]
        [Range(0.01, 100, ErrorMessage = "PercentageOfBenefit must be between 0.01 and 100")]
        public decimal PercentageOfBenefit { get; set; }
    }
}