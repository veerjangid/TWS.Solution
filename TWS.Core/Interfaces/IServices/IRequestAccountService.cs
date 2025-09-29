using TWS.Core.DTOs.Request.Account;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Account;

namespace TWS.Core.Interfaces.IServices
{
    /// <summary>
    /// Service interface for account request operations
    /// Handles business logic for processing account requests from potential investors
    /// Reference: BusinessRequirement.md Section 3.1, FunctionalRequirement.md
    /// </summary>
    public interface IRequestAccountService
    {
        /// <summary>
        /// Creates a new account request from a potential investor
        /// </summary>
        /// <param name="request">Account request details</param>
        /// <returns>ApiResponse with created AccountRequestResponse</returns>
        Task<ApiResponse<AccountRequestResponse>> CreateRequestAsync(CreateAccountRequestRequest request);

        /// <summary>
        /// Retrieves all account requests ordered by request date descending
        /// </summary>
        /// <returns>ApiResponse with list of AccountRequestResponse</returns>
        Task<ApiResponse<List<AccountRequestResponse>>> GetAllRequestsAsync();

        /// <summary>
        /// Retrieves a specific account request by ID
        /// </summary>
        /// <param name="id">Account request ID</param>
        /// <returns>ApiResponse with AccountRequestResponse or error if not found</returns>
        Task<ApiResponse<AccountRequestResponse>> GetRequestByIdAsync(int id);

        /// <summary>
        /// Processes an account request by marking it as processed
        /// Updates ProcessedDate, ProcessedByUserId, and optional notes
        /// </summary>
        /// <param name="id">Account request ID</param>
        /// <param name="request">Processing details including notes</param>
        /// <param name="userId">ID of the user processing the request</param>
        /// <returns>ApiResponse with updated AccountRequestResponse</returns>
        Task<ApiResponse<AccountRequestResponse>> ProcessRequestAsync(int id, ProcessAccountRequestRequest request, string userId);

        /// <summary>
        /// Deletes an account request by ID
        /// </summary>
        /// <param name="id">Account request ID</param>
        /// <returns>ApiResponse with boolean indicating success</returns>
        Task<ApiResponse<bool>> DeleteRequestAsync(int id);
    }
}