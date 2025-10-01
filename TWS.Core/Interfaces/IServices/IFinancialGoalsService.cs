using TWS.Core.DTOs.Request.FinancialGoals;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.FinancialGoals;

namespace TWS.Core.Interfaces.IServices
{
    /// <summary>
    /// Service interface for Financial Goals operations.
    /// Handles creation, retrieval, update, and deletion of investor financial goals.
    /// Reference: APIDoc.md Section 9, BusinessRequirement.md Section 10
    /// </summary>
    public interface IFinancialGoalsService
    {
        /// <summary>
        /// Saves financial goals for an investor profile.
        /// Creates new record if none exists, updates existing record if one exists (one-to-one relationship).
        /// </summary>
        /// <param name="request">The financial goals data to save.</param>
        /// <returns>ApiResponse containing the saved financial goals details.</returns>
        Task<ApiResponse<FinancialGoalsResponse>> SaveFinancialGoalsAsync(SaveFinancialGoalsRequest request);

        /// <summary>
        /// Retrieves the financial goals for a specific investor profile.
        /// </summary>
        /// <param name="investorProfileId">The ID of the investor profile.</param>
        /// <returns>ApiResponse containing the financial goals details, or NotFound if none exists.</returns>
        Task<ApiResponse<FinancialGoalsResponse>> GetByInvestorProfileIdAsync(int investorProfileId);

        /// <summary>
        /// Deletes the financial goals for a specific investor profile.
        /// </summary>
        /// <param name="investorProfileId">The ID of the investor profile.</param>
        /// <returns>ApiResponse indicating success or failure.</returns>
        Task<ApiResponse<bool>> DeleteFinancialGoalsAsync(int investorProfileId);
    }
}