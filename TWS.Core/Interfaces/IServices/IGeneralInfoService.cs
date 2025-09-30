using TWS.Core.DTOs.Request.GeneralInfo;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.GeneralInfo;

namespace TWS.Core.Interfaces.IServices
{
    public interface IGeneralInfoService
    {
        /// <summary>
        /// Saves or updates Individual General Info for an investor profile
        /// </summary>
        Task<ApiResponse<GeneralInfoResponse>> SaveIndividualGeneralInfoAsync(SaveIndividualGeneralInfoRequest request);

        /// <summary>
        /// Saves or updates Joint General Info for an investor profile
        /// </summary>
        Task<ApiResponse<GeneralInfoResponse>> SaveJointGeneralInfoAsync(SaveJointGeneralInfoRequest request);

        /// <summary>
        /// Adds a Joint Account Holder to existing Joint General Info
        /// </summary>
        Task<ApiResponse<JointAccountHolderResponse>> AddJointAccountHolderAsync(AddJointAccountHolderRequest request);

        /// <summary>
        /// Saves or updates IRA General Info for an investor profile
        /// </summary>
        Task<ApiResponse<GeneralInfoResponse>> SaveIRAGeneralInfoAsync(SaveIRAGeneralInfoRequest request);

        /// <summary>
        /// Saves or updates Trust General Info for an investor profile
        /// </summary>
        Task<ApiResponse<GeneralInfoResponse>> SaveTrustGeneralInfoAsync(SaveTrustGeneralInfoRequest request);

        /// <summary>
        /// Adds a Trust Grantor to existing Trust General Info
        /// </summary>
        Task<ApiResponse<TrustGrantorResponse>> AddTrustGrantorAsync(AddTrustGrantorRequest request);

        /// <summary>
        /// Saves or updates Entity General Info for an investor profile
        /// </summary>
        Task<ApiResponse<GeneralInfoResponse>> SaveEntityGeneralInfoAsync(SaveEntityGeneralInfoRequest request);

        /// <summary>
        /// Adds an Entity Equity Owner to existing Entity General Info
        /// </summary>
        Task<ApiResponse<EntityEquityOwnerResponse>> AddEntityEquityOwnerAsync(AddEntityEquityOwnerRequest request);

        /// <summary>
        /// Gets General Info by Investor Profile ID with type-specific data
        /// </summary>
        Task<ApiResponse<GeneralInfoResponse>> GetGeneralInfoByInvestorProfileIdAsync(int investorProfileId);
    }
}