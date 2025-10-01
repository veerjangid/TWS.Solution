using System;
using System.ComponentModel.DataAnnotations;

namespace TWS.Data.Entities.PrimaryInvestorInfo
{
    /// <summary>
    /// Broker Affiliation entity containing broker-dealer relationship information
    /// Table 19 in DatabaseSchema.md
    /// </summary>
    public class BrokerAffiliation
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to PrimaryInvestorInfo
        /// </summary>
        [Required]
        public int PrimaryInvestorInfoId { get; set; }

        /// <summary>
        /// Indicates if investor is employee of broker-dealer
        /// </summary>
        [Required]
        public bool IsEmployeeOfBrokerDealer { get; set; }

        /// <summary>
        /// Name of broker-dealer (if employee)
        /// </summary>
        [MaxLength(200)]
        public string? BrokerDealerName { get; set; }

        /// <summary>
        /// Indicates if investor is related to employee of broker-dealer
        /// </summary>
        [Required]
        public bool IsRelatedToEmployee { get; set; }

        /// <summary>
        /// Name of related broker-dealer
        /// </summary>
        [MaxLength(200)]
        public string? RelatedBrokerDealerName { get; set; }

        /// <summary>
        /// Name of the employee
        /// </summary>
        [MaxLength(200)]
        public string? EmployeeName { get; set; }

        /// <summary>
        /// Relationship to the employee
        /// </summary>
        [MaxLength(100)]
        public string? Relationship { get; set; }

        /// <summary>
        /// Indicates if investor is senior officer of publicly owned company
        /// </summary>
        [Required]
        public bool IsSeniorOfficer { get; set; }

        /// <summary>
        /// Indicates if investor is manager/member/executive of non-public company
        /// </summary>
        [Required]
        public bool IsManagerMemberExecutive { get; set; }

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
        /// Navigation property to PrimaryInvestorInfo
        /// </summary>
        public PrimaryInvestorInfo PrimaryInvestorInfo { get; set; } = null!;
    }
}