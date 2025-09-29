using TWS.Core.DTOs.Request.Investor;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Investor;

namespace TWS.Core.Interfaces.IServices
{
    /// <summary>
    /// Service interface for investor profile operations
    /// </summary>
    public interface IInvestorService
    {
        /// <summary>
        /// Select Individual investor type and create profile
        /// </summary>
        Task<ApiResponse<InvestorProfileResponse>> SelectInvestorTypeIndividualAsync(string userId, SelectInvestorTypeIndividualRequest request);

        /// <summary>
        /// Select Joint investor type and create profile
        /// </summary>
        Task<ApiResponse<InvestorProfileResponse>> SelectInvestorTypeJointAsync(string userId, SelectInvestorTypeJointRequest request);

        /// <summary>
        /// Select IRA investor type and create profile
        /// </summary>
        Task<ApiResponse<InvestorProfileResponse>> SelectInvestorTypeIRAAsync(string userId, SelectInvestorTypeIRARequest request);

        /// <summary>
        /// Select Trust investor type and create profile
        /// </summary>
        Task<ApiResponse<InvestorProfileResponse>> SelectInvestorTypeTrustAsync(string userId, SelectInvestorTypeTrustRequest request);

        /// <summary>
        /// Select Entity investor type and create profile
        /// </summary>
        Task<ApiResponse<InvestorProfileResponse>> SelectInvestorTypeEntityAsync(string userId, SelectInvestorTypeEntityRequest request);

        /// <summary>
        /// Get investor profile by user ID
        /// </summary>
        Task<ApiResponse<InvestorProfileResponse>> GetInvestorProfileAsync(string userId);

        /// <summary>
        /// Get investor profile by profile ID
        /// </summary>
        Task<ApiResponse<InvestorProfileResponse>> GetInvestorProfileByIdAsync(int id);

        /// <summary>
        /// Update accreditation status
        /// </summary>
        Task<ApiResponse<bool>> UpdateAccreditationAsync(int investorProfileId, bool isAccredited, int? accreditationType);
    }
}