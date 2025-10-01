using TWS.Core.DTOs.Request.PrimaryInvestorInfo;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.PrimaryInvestorInfo;

namespace TWS.Core.Interfaces.IServices
{
    /// <summary>
    /// Service interface for Primary Investor Info business logic
    /// </summary>
    public interface IPrimaryInvestorInfoService
    {
        /// <summary>
        /// Saves or updates the primary investor information
        /// </summary>
        /// <param name="request">The primary investor info data</param>
        /// <returns>The saved primary investor info response</returns>
        Task<ApiResponse<PrimaryInvestorInfoResponse>> SavePrimaryInvestorInfoAsync(SavePrimaryInvestorInfoRequest request);

        /// <summary>
        /// Saves or updates the broker affiliation information
        /// </summary>
        /// <param name="request">The broker affiliation data</param>
        /// <returns>The saved broker affiliation response</returns>
        Task<ApiResponse<BrokerAffiliationResponse>> SaveBrokerAffiliationAsync(SaveBrokerAffiliationRequest request);

        /// <summary>
        /// Saves investment experience information (replaces existing)
        /// </summary>
        /// <param name="request">The investment experience data</param>
        /// <returns>The saved investment experiences</returns>
        Task<ApiResponse<List<InvestmentExperienceResponse>>> SaveInvestmentExperienceAsync(SaveInvestmentExperienceRequest request);

        /// <summary>
        /// Saves source of funds information (replaces existing)
        /// </summary>
        /// <param name="request">The source of funds data</param>
        /// <returns>The saved source of funds</returns>
        Task<ApiResponse<List<SourceOfFundsResponse>>> SaveSourceOfFundsAsync(SaveSourceOfFundsRequest request);

        /// <summary>
        /// Saves tax rates information (replaces existing)
        /// </summary>
        /// <param name="request">The tax rates data</param>
        /// <returns>The saved tax rates</returns>
        Task<ApiResponse<List<TaxRateResponse>>> SaveTaxRatesAsync(SaveTaxRatesRequest request);

        /// <summary>
        /// Gets primary investor info by investor profile ID
        /// </summary>
        /// <param name="investorProfileId">The investor profile identifier</param>
        /// <returns>The primary investor info with all related data</returns>
        Task<ApiResponse<PrimaryInvestorInfoResponse>> GetByInvestorProfileIdAsync(int investorProfileId);
    }
}