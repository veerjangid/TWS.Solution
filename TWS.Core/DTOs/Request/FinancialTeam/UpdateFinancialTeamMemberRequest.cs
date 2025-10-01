using System.ComponentModel.DataAnnotations;

namespace TWS.Core.DTOs.Request.FinancialTeam
{
    /// <summary>
    /// Request DTO for updating an existing financial team member
    /// Reference: DatabaseSchema.md Table 29, APIDoc.md Section 11
    /// Note: InvestorProfileId and MemberType cannot be changed after creation
    /// </summary>
    public class UpdateFinancialTeamMemberRequest
    {
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