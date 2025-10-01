using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.FinancialTeam
{
    /// <summary>
    /// Request DTO for adding a new financial team member to an investor profile
    /// Reference: DatabaseSchema.md Table 29, APIDoc.md Section 11
    /// </summary>
    public class AddFinancialTeamMemberRequest
    {
        /// <summary>
        /// ID of the investor profile
        /// </summary>
        [Required(ErrorMessage = "InvestorProfileId is required")]
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Type of team member (1=Accountant, 2=Attorney, 3=FinancialAdvisor, 4=InsuranceAgent, 5=Other)
        /// </summary>
        [Required(ErrorMessage = "MemberType is required")]
        [Range(1, 5, ErrorMessage = "MemberType must be between 1 and 5")]
        public int MemberType { get; set; }

        /// <summary>
        /// Full name of the team member
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; }

        /// <summary>
        /// Email address of the team member
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        [MaxLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
        public string Email { get; set; }

        /// <summary>
        /// Phone number of the team member
        /// </summary>
        [Required(ErrorMessage = "PhoneNumber is required")]
        [MaxLength(20, ErrorMessage = "PhoneNumber cannot exceed 20 characters")]
        public string PhoneNumber { get; set; }
    }
}