using TWS.Core.DTOs.Request.Investment;
using TWS.Core.DTOs.Response.Investment;

namespace TWS.Core.Interfaces.IServices
{
    /// <summary>
    /// Service interface for Investment management
    /// Handles business logic for investor investments in offerings
    /// Reference: APIDoc.md Section 12
    /// </summary>
    public interface IInvestmentService
    {
        /// <summary>
        /// Creates a new investment
        /// </summary>
        /// <param name="request">Investment creation request</param>
        /// <returns>Created investment details</returns>
        Task<InvestmentResponse> CreateInvestmentAsync(CreateInvestmentRequest request);

        /// <summary>
        /// Updates investment status
        /// </summary>
        /// <param name="investmentId">Investment ID</param>
        /// <param name="request">Status update request</param>
        /// <returns>Updated investment details</returns>
        Task<InvestmentResponse> UpdateStatusAsync(int investmentId, UpdateInvestmentStatusRequest request);

        /// <summary>
        /// Gets all investments for a specific investor profile
        /// </summary>
        /// <param name="investorProfileId">Investor profile ID</param>
        /// <returns>Collection of investments for the investor</returns>
        Task<IEnumerable<InvestmentResponse>> GetByInvestorProfileIdAsync(int investorProfileId);

        /// <summary>
        /// Gets detailed information about an investment
        /// </summary>
        /// <param name="investmentId">Investment ID</param>
        /// <returns>Investment details with offering information</returns>
        Task<InvestmentDetailsResponse?> GetInvestmentDetailsAsync(int investmentId);
    }
}