namespace TWS.Core.DTOs.Response.PrimaryInvestorInfo
{
    /// <summary>
    /// Response DTO for Primary Investor Info
    /// </summary>
    public class PrimaryInvestorInfoResponse
    {
        /// <summary>
        /// Gets or sets the primary investor info identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the investor profile identifier
        /// </summary>
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the legal street address
        /// </summary>
        public string LegalStreetAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the city
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the state
        /// </summary>
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ZIP code
        /// </summary>
        public string Zip { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the cell phone number
        /// </summary>
        public string CellPhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the investor is married
        /// </summary>
        public bool IsMarried { get; set; }

        /// <summary>
        /// Gets or sets the masked Social Security Number (***-**-1234)
        /// </summary>
        public string SocialSecurityNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the driver's license number
        /// </summary>
        public string DriversLicenseNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the driver's license expiration date
        /// </summary>
        public DateTime DriversLicenseExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the occupation
        /// </summary>
        public string Occupation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the employer name
        /// </summary>
        public string? EmployerName { get; set; }

        /// <summary>
        /// Gets or sets the retired profession
        /// </summary>
        public string? RetiredProfession { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the investor has an alternate address
        /// </summary>
        public bool HasAlternateAddress { get; set; }

        /// <summary>
        /// Gets or sets the alternate address
        /// </summary>
        public string? AlternateAddress { get; set; }

        /// <summary>
        /// Gets or sets the lowest income in the last two years
        /// </summary>
        public decimal LowestIncomeLastTwoYears { get; set; }

        /// <summary>
        /// Gets or sets the anticipated income this year
        /// </summary>
        public decimal AnticipatedIncomeThisYear { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the investor is relying on joint income
        /// </summary>
        public bool IsRelyingOnJointIncome { get; set; }

        /// <summary>
        /// Gets or sets the broker affiliation information
        /// </summary>
        public BrokerAffiliationResponse? BrokerAffiliation { get; set; }

        /// <summary>
        /// Gets or sets the investment experiences
        /// </summary>
        public List<InvestmentExperienceResponse> InvestmentExperiences { get; set; } = new List<InvestmentExperienceResponse>();

        /// <summary>
        /// Gets or sets the source of funds
        /// </summary>
        public List<SourceOfFundsResponse> SourceOfFunds { get; set; } = new List<SourceOfFundsResponse>();

        /// <summary>
        /// Gets or sets the tax rates
        /// </summary>
        public List<TaxRateResponse> TaxRates { get; set; } = new List<TaxRateResponse>();

        /// <summary>
        /// Gets or sets the creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the last update timestamp
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}