using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.PrimaryInvestorInfo
{
    /// <summary>
    /// Request DTO for saving Broker Affiliation information
    /// </summary>
    public class SaveBrokerAffiliationRequest
    {
        /// <summary>
        /// Gets or sets the primary investor info identifier
        /// </summary>
        [Required(ErrorMessage = "Primary Investor Info ID is required")]
        public int PrimaryInvestorInfoId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the investor is an employee of a broker-dealer
        /// </summary>
        [Required(ErrorMessage = "Employee of broker-dealer flag is required")]
        public bool IsEmployeeOfBrokerDealer { get; set; }

        /// <summary>
        /// Gets or sets the broker-dealer name (optional)
        /// </summary>
        [StringLength(200, ErrorMessage = "Broker-dealer name cannot exceed 200 characters")]
        public string? BrokerDealerName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the investor is related to an employee
        /// </summary>
        [Required(ErrorMessage = "Related to employee flag is required")]
        public bool IsRelatedToEmployee { get; set; }

        /// <summary>
        /// Gets or sets the related broker-dealer name (optional)
        /// </summary>
        [StringLength(200, ErrorMessage = "Related broker-dealer name cannot exceed 200 characters")]
        public string? RelatedBrokerDealerName { get; set; }

        /// <summary>
        /// Gets or sets the employee name (optional)
        /// </summary>
        [StringLength(200, ErrorMessage = "Employee name cannot exceed 200 characters")]
        public string? EmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the relationship (optional)
        /// </summary>
        [StringLength(100, ErrorMessage = "Relationship cannot exceed 100 characters")]
        public string? Relationship { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the investor is a senior officer
        /// </summary>
        [Required(ErrorMessage = "Senior officer flag is required")]
        public bool IsSeniorOfficer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the investor is a manager/member/executive
        /// </summary>
        [Required(ErrorMessage = "Manager/Member/Executive flag is required")]
        public bool IsManagerMemberExecutive { get; set; }
    }
}