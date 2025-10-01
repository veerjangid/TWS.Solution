using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.PrimaryInvestorInfo
{
    /// <summary>
    /// Request DTO for saving Primary Investor Info
    /// </summary>
    public class SavePrimaryInvestorInfoRequest
    {
        /// <summary>
        /// Gets or sets the investor profile identifier
        /// </summary>
        [Required(ErrorMessage = "Investor Profile ID is required")]
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the legal street address
        /// </summary>
        [Required(ErrorMessage = "Legal street address is required")]
        [StringLength(500, ErrorMessage = "Legal street address cannot exceed 500 characters")]
        public string LegalStreetAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the city
        /// </summary>
        [Required(ErrorMessage = "City is required")]
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the state
        /// </summary>
        [Required(ErrorMessage = "State is required")]
        [StringLength(50, ErrorMessage = "State cannot exceed 50 characters")]
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ZIP code
        /// </summary>
        [Required(ErrorMessage = "ZIP code is required")]
        [StringLength(10, ErrorMessage = "ZIP code cannot exceed 10 characters")]
        public string Zip { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the cell phone number
        /// </summary>
        [Required(ErrorMessage = "Cell phone number is required")]
        [StringLength(20, ErrorMessage = "Cell phone number cannot exceed 20 characters")]
        public string CellPhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the investor is married
        /// </summary>
        [Required(ErrorMessage = "Marital status is required")]
        public bool IsMarried { get; set; }

        /// <summary>
        /// Gets or sets the Social Security Number (Format: XXX-XX-XXXX)
        /// </summary>
        [Required(ErrorMessage = "Social Security Number is required")]
        [StringLength(11, ErrorMessage = "Social Security Number must be 11 characters")]
        [RegularExpression(@"^\d{3}-\d{2}-\d{4}$", ErrorMessage = "Social Security Number must be in format XXX-XX-XXXX")]
        public string SocialSecurityNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date of birth
        /// </summary>
        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the driver's license number
        /// </summary>
        [Required(ErrorMessage = "Driver's license number is required")]
        [StringLength(50, ErrorMessage = "Driver's license number cannot exceed 50 characters")]
        public string DriversLicenseNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the driver's license expiration date
        /// </summary>
        [Required(ErrorMessage = "Driver's license expiration date is required")]
        public DateTime DriversLicenseExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the occupation
        /// </summary>
        [Required(ErrorMessage = "Occupation is required")]
        [StringLength(200, ErrorMessage = "Occupation cannot exceed 200 characters")]
        public string Occupation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the employer name (optional)
        /// </summary>
        [StringLength(200, ErrorMessage = "Employer name cannot exceed 200 characters")]
        public string? EmployerName { get; set; }

        /// <summary>
        /// Gets or sets the retired profession (optional)
        /// </summary>
        [StringLength(200, ErrorMessage = "Retired profession cannot exceed 200 characters")]
        public string? RetiredProfession { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the investor has an alternate address
        /// </summary>
        [Required(ErrorMessage = "Alternate address flag is required")]
        public bool HasAlternateAddress { get; set; }

        /// <summary>
        /// Gets or sets the alternate address (optional)
        /// </summary>
        [StringLength(500, ErrorMessage = "Alternate address cannot exceed 500 characters")]
        public string? AlternateAddress { get; set; }

        /// <summary>
        /// Gets or sets the lowest income in the last two years
        /// </summary>
        [Required(ErrorMessage = "Lowest income in last two years is required")]
        [Range(0, 999999999, ErrorMessage = "Income must be between 0 and 999,999,999")]
        public decimal LowestIncomeLastTwoYears { get; set; }

        /// <summary>
        /// Gets or sets the anticipated income this year
        /// </summary>
        [Required(ErrorMessage = "Anticipated income this year is required")]
        [Range(0, 999999999, ErrorMessage = "Income must be between 0 and 999,999,999")]
        public decimal AnticipatedIncomeThisYear { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the investor is relying on joint income
        /// </summary>
        [Required(ErrorMessage = "Joint income flag is required")]
        public bool IsRelyingOnJointIncome { get; set; }
    }
}