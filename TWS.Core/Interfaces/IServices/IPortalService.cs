using TWS.Core.DTOs.Request.Portal;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Portal;

namespace TWS.Core.Interfaces.IServices
{
    /// <summary>
    /// Service interface for Portal/CRM module
    /// Handles business logic for investment tracking with financial metrics
    /// Reference: APIDoc.md Section 13, DatabaseSchema.md Table 32
    /// </summary>
    public interface IPortalService
    {
        /// <summary>
        /// Creates a new investment tracker
        /// </summary>
        /// <param name="request">Investment tracker creation request</param>
        /// <returns>Created tracker with ID</returns>
        Task<ApiResponse<InvestmentTrackerResponse>> CreateTrackerAsync(CreateInvestmentTrackerRequest request);

        /// <summary>
        /// Updates the status of an investment tracker
        /// </summary>
        /// <param name="id">Tracker ID</param>
        /// <param name="request">Status update request</param>
        /// <returns>Updated tracker details</returns>
        Task<ApiResponse<InvestmentTrackerResponse>> UpdateStatusAsync(int id, UpdateTrackerStatusRequest request);

        /// <summary>
        /// Gets all trackers for dashboard view
        /// Used for Portal CRM dashboard display
        /// </summary>
        /// <returns>Collection of dashboard items</returns>
        Task<ApiResponse<List<DashboardItemResponse>>> GetDashboardAsync();

        /// <summary>
        /// Gets a specific tracker by ID with full details
        /// </summary>
        /// <param name="id">Tracker ID</param>
        /// <returns>Tracker details if found</returns>
        Task<ApiResponse<InvestmentTrackerResponse>> GetTrackerByIdAsync(int id);

        /// <summary>
        /// Deletes an investment tracker
        /// </summary>
        /// <param name="id">Tracker ID</param>
        /// <returns>Success status</returns>
        Task<ApiResponse<bool>> DeleteTrackerAsync(int id);
    }
}