using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.GeneralInfo
{
    public class SaveIRAGeneralInfoRequest
    {
        [Required(ErrorMessage = "Investor Profile ID is required")]
        public int InvestorProfileId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "SSN is required")]
        [MaxLength(11, ErrorMessage = "SSN cannot exceed 11 characters")]
        [RegularExpression(@"^\d{3}-\d{2}-\d{4}$", ErrorMessage = "SSN must be in format XXX-XX-XXXX")]
        public string SSN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        [MaxLength(20, ErrorMessage = "Phone cannot exceed 20 characters")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Custodian Name is required")]
        [MaxLength(200, ErrorMessage = "Custodian Name cannot exceed 200 characters")]
        public string CustodianName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account Type is required")]
        [Range(1, 5, ErrorMessage = "Account Type must be one of the 5 valid IRA types")]
        public int AccountType { get; set; }

        [Required(ErrorMessage = "IRA Account Number is required")]
        [MaxLength(100, ErrorMessage = "IRA Account Number cannot exceed 100 characters")]
        public string IRAAccountNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "IsRollingOverToCNB flag is required")]
        public bool IsRollingOverToCNB { get; set; }

        [Required(ErrorMessage = "Custodian Phone Number is required")]
        [MaxLength(20, ErrorMessage = "Custodian Phone Number cannot exceed 20 characters")]
        public string CustodianPhoneNumber { get; set; } = string.Empty;

        [MaxLength(20, ErrorMessage = "Custodian Fax Number cannot exceed 20 characters")]
        public string? CustodianFaxNumber { get; set; }

        [Required(ErrorMessage = "HasLiquidatedAssets flag is required")]
        public bool HasLiquidatedAssets { get; set; }
    }
}