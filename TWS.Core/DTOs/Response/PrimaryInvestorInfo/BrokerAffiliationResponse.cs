namespace TWS.Core.DTOs.Response.PrimaryInvestorInfo
{
    /// <summary>
    /// Response DTO for Broker Affiliation information
    /// </summary>
    public class BrokerAffiliationResponse
    {
        /// <summary>
        /// Gets or sets the broker affiliation identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the primary investor info identifier
        /// </summary>
        public int PrimaryInvestorInfoId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the investor is an employee of a broker-dealer
        /// </summary>
        public bool IsEmployeeOfBrokerDealer { get; set; }

        /// <summary>
        /// Gets or sets the broker-dealer name
        /// </summary>
        public string? BrokerDealerName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the investor is related to an employee
        /// </summary>
        public bool IsRelatedToEmployee { get; set; }

        /// <summary>
        /// Gets or sets the related broker-dealer name
        /// </summary>
        public string? RelatedBrokerDealerName { get; set; }

        /// <summary>
        /// Gets or sets the employee name
        /// </summary>
        public string? EmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the relationship
        /// </summary>
        public string? Relationship { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the investor is a senior officer
        /// </summary>
        public bool IsSeniorOfficer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the investor is a manager/member/executive
        /// </summary>
        public bool IsManagerMemberExecutive { get; set; }

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