using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.PersonalFinancialStatement;

namespace TWS.Core.Interfaces.IServices
{
    /// <summary>
    /// Service interface for Personal Financial Statement operations.
    /// Handles upload, retrieval, and deletion of PFS documents.
    /// Reference: APIDoc.md Section 8, BusinessRequirement.md Section 9
    /// </summary>
    public interface IPersonalFinancialStatementService
    {
        /// <summary>
        /// Uploads a new Personal Financial Statement for an investor profile.
        /// Replaces existing PFS if one exists (one-to-one relationship).
        /// </summary>
        /// <param name="investorProfileId">The ID of the investor profile.</param>
        /// <param name="filePath">The path where the uploaded PDF file is stored.</param>
        /// <returns>ApiResponse containing the uploaded PFS details.</returns>
        Task<ApiResponse<PFSResponse>> UploadPFSAsync(int investorProfileId, string filePath);

        /// <summary>
        /// Retrieves the Personal Financial Statement for a specific investor profile.
        /// </summary>
        /// <param name="investorProfileId">The ID of the investor profile.</param>
        /// <returns>ApiResponse containing the PFS details, or NotFound if none exists.</returns>
        Task<ApiResponse<PFSResponse>> GetByInvestorProfileIdAsync(int investorProfileId);

        /// <summary>
        /// Deletes the Personal Financial Statement for a specific investor profile.
        /// Also removes the physical file from storage.
        /// </summary>
        /// <param name="investorProfileId">The ID of the investor profile.</param>
        /// <returns>ApiResponse indicating success or failure.</returns>
        Task<ApiResponse<bool>> DeletePFSAsync(int investorProfileId);
    }
}