using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TWS.Data.Entities.Core;

namespace TWS.Data.Entities.PrimaryInvestorInfo
{
    /// <summary>
    /// Primary Investor Information entity containing personal and financial details
    /// Table 18 in DatabaseSchema.md
    /// </summary>
    public class PrimaryInvestorInfo
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to InvestorProfile - One-to-one relationship
        /// </summary>
        [Required]
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// First name of primary investor
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of primary investor
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Legal street address
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string LegalStreetAddress { get; set; } = string.Empty;

        /// <summary>
        /// City
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// State
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// ZIP code
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string Zip { get; set; } = string.Empty;

        /// <summary>
        /// Email address
        /// </summary>
        [Required]
        [MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Cell phone number
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string CellPhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Marital status
        /// </summary>
        [Required]
        public bool IsMarried { get; set; }

        /// <summary>
        /// Social Security Number (encrypted at field level using AES-256)
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string SocialSecurityNumber { get; set; } = string.Empty;

        /// <summary>
        /// Date of birth
        /// </summary>
        [Required]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Driver's license number
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string DriversLicenseNumber { get; set; } = string.Empty;

        /// <summary>
        /// Driver's license expiration date
        /// </summary>
        [Required]
        public DateTime DriversLicenseExpirationDate { get; set; }

        /// <summary>
        /// Current occupation
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Occupation { get; set; } = string.Empty;

        /// <summary>
        /// Employer name (if employed)
        /// </summary>
        [MaxLength(200)]
        public string? EmployerName { get; set; }

        /// <summary>
        /// Retired profession (if retired)
        /// </summary>
        [MaxLength(200)]
        public string? RetiredProfession { get; set; }

        /// <summary>
        /// Indicates if investor has an alternate address
        /// </summary>
        [Required]
        public bool HasAlternateAddress { get; set; }

        /// <summary>
        /// Alternate address (if applicable)
        /// </summary>
        [MaxLength(500)]
        public string? AlternateAddress { get; set; }

        /// <summary>
        /// Lowest income in the last two years
        /// </summary>
        [Required]
        public decimal LowestIncomeLastTwoYears { get; set; }

        /// <summary>
        /// Anticipated income for this year
        /// </summary>
        [Required]
        public decimal AnticipatedIncomeThisYear { get; set; }

        /// <summary>
        /// Indicates if relying on joint income
        /// </summary>
        [Required]
        public bool IsRelyingOnJointIncome { get; set; }

        /// <summary>
        /// Record creation timestamp
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Record last update timestamp
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties

        /// <summary>
        /// Navigation property to InvestorProfile
        /// </summary>
        public InvestorProfile InvestorProfile { get; set; } = null!;

        /// <summary>
        /// Collection of broker affiliations
        /// </summary>
        public ICollection<BrokerAffiliation> BrokerAffiliations { get; set; } = new List<BrokerAffiliation>();

        /// <summary>
        /// Collection of investment experiences
        /// </summary>
        public ICollection<InvestmentExperience> InvestmentExperiences { get; set; } = new List<InvestmentExperience>();

        /// <summary>
        /// Collection of source of funds
        /// </summary>
        public ICollection<SourceOfFunds> SourceOfFunds { get; set; } = new List<SourceOfFunds>();

        /// <summary>
        /// Collection of tax rates
        /// </summary>
        public ICollection<TaxRate> TaxRates { get; set; } = new List<TaxRate>();
    }
}