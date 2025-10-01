namespace TWS.Core.DTOs.Response.FinancialTeam
{
    /// <summary>
    /// Response DTO for financial team member data
    /// Reference: DatabaseSchema.md Table 29, APIDoc.md Section 11
    /// </summary>
    public class FinancialTeamMemberResponse
    {
        /// <summary>
        /// Unique identifier for the team member
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID of the associated investor profile
        /// </summary>
        public int InvestorProfileId { get; set; }

        /// <summary>
        /// Type of team member as integer (1-5)
        /// </summary>
        public int MemberType { get; set; }

        /// <summary>
        /// Type of team member as string (e.g., "Accountant", "Attorney")
        /// </summary>
        public string MemberTypeName { get; set; }

        /// <summary>
        /// Full name of the team member
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email address of the team member
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Phone number of the team member
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Timestamp when the record was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Timestamp when the record was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}