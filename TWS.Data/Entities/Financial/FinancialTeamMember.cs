using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWS.Core.Enums;
using TWS.Data.Entities.Core;

namespace TWS.Data.Entities.Financial
{
    /// <summary>
    /// Represents a financial team member (accountant, attorney, etc.) associated with an investor profile
    /// Table 29 in DatabaseSchema.md
    /// </summary>
    [Table("FinancialTeamMembers")]
    public class FinancialTeamMember
    {
        /// <summary>
        /// Primary key - Auto-increment
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to InvestorProfile
        /// </summary>
        [Required]
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Type of team member (Accountant, Attorney, FinancialAdvisor, InsuranceAgent, Other)
        /// </summary>
        [Required]
        public FinancialTeamMemberType MemberType { get; set; }

        /// <summary>
        /// Full name of the team member
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// Email address of the team member
        /// </summary>
        [Required]
        [MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Phone number of the team member
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Timestamp when the record was created
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Timestamp when the record was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties

        /// <summary>
        /// Navigation to the associated InvestorProfile
        /// </summary>
        [ForeignKey(nameof(InvestorProfileId))]
        public virtual InvestorProfile InvestorProfile { get; set; }
    }
}