using TWS.Core.DTOs.Request.FinancialTeam;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.FinancialTeam;

namespace TWS.Core.Interfaces.IServices
{
    /// <summary>
    /// Service interface for managing financial team members
    /// Reference: DatabaseSchema.md Table 29, APIDoc.md Section 11
    /// </summary>
    public interface IFinancialTeamService
    {
        /// <summary>
        /// Adds a new financial team member to an investor profile
        /// </summary>
        /// <param name="request">The financial team member details</param>
        /// <returns>ApiResponse with the created team member</returns>
        Task<ApiResponse<FinancialTeamMemberResponse>> AddMemberAsync(AddFinancialTeamMemberRequest request);

        /// <summary>
        /// Updates an existing financial team member
        /// </summary>
        /// <param name="id">The ID of the team member to update</param>
        /// <param name="request">The updated team member details</param>
        /// <returns>ApiResponse with the updated team member</returns>
        Task<ApiResponse<FinancialTeamMemberResponse>> UpdateMemberAsync(int id, UpdateFinancialTeamMemberRequest request);

        /// <summary>
        /// Deletes a financial team member
        /// </summary>
        /// <param name="id">The ID of the team member to delete</param>
        /// <returns>ApiResponse with success status</returns>
        Task<ApiResponse<bool>> DeleteMemberAsync(int id);

        /// <summary>
        /// Gets all financial team members for an investor profile
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <returns>ApiResponse with list of team members</returns>
        Task<ApiResponse<List<FinancialTeamMemberResponse>>> GetByInvestorProfileIdAsync(int investorProfileId);

        /// <summary>
        /// Gets a financial team member by ID
        /// </summary>
        /// <param name="id">The team member ID</param>
        /// <returns>ApiResponse with the team member details</returns>
        Task<ApiResponse<FinancialTeamMemberResponse>> GetByIdAsync(int id);
    }
}