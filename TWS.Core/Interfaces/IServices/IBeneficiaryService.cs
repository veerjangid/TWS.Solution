using TWS.Core.DTOs.Request.Beneficiary;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Beneficiary;

namespace TWS.Core.Interfaces.IServices
{
    public interface IBeneficiaryService
    {
        /// <summary>
        /// Adds a single beneficiary to an investor profile
        /// </summary>
        Task<ApiResponse<BeneficiaryResponse>> AddBeneficiaryAsync(AddBeneficiaryRequest request);

        /// <summary>
        /// Adds multiple beneficiaries to an investor profile (replaces existing beneficiaries of the same types)
        /// </summary>
        Task<ApiResponse<List<BeneficiaryResponse>>> AddMultipleBeneficiariesAsync(AddMultipleBeneficiariesRequest request);

        /// <summary>
        /// Updates an existing beneficiary
        /// </summary>
        Task<ApiResponse<BeneficiaryResponse>> UpdateBeneficiaryAsync(int id, UpdateBeneficiaryRequest request);

        /// <summary>
        /// Deletes a beneficiary by ID
        /// </summary>
        Task<ApiResponse<bool>> DeleteBeneficiaryAsync(int id);

        /// <summary>
        /// Gets all beneficiaries for an investor profile
        /// </summary>
        Task<ApiResponse<List<BeneficiaryResponse>>> GetByInvestorProfileIdAsync(int investorProfileId);

        /// <summary>
        /// Gets beneficiaries grouped by type (Primary and Contingent) with total percentages
        /// </summary>
        Task<ApiResponse<BeneficiariesGroupedResponse>> GetGroupedByInvestorProfileIdAsync(int investorProfileId);
    }
}