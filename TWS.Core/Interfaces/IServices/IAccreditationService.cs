using TWS.Core.DTOs.Request.Accreditation;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Accreditation;

namespace TWS.Core.Interfaces.IServices
{
    /// <summary>
    /// Service interface for investor accreditation management
    /// </summary>
    public interface IAccreditationService
    {
        /// <summary>
        /// Saves investor accreditation information (create or update)
        /// </summary>
        /// <param name="request">Accreditation save request</param>
        /// <returns>Accreditation response with status code 201 (Created) or 200 (OK)</returns>
        Task<ApiResponse<AccreditationResponse>> SaveAccreditationAsync(SaveAccreditationRequest request);

        /// <summary>
        /// Uploads an accreditation document for an investor
        /// </summary>
        /// <param name="investorAccreditationId">The investor accreditation ID</param>
        /// <param name="documentType">Type of document being uploaded</param>
        /// <param name="filePath">Path where the document is stored</param>
        /// <returns>Accreditation document response with status code 201 (Created)</returns>
        Task<ApiResponse<AccreditationDocumentResponse>> UploadDocumentAsync(int investorAccreditationId, string documentType, string filePath);

        /// <summary>
        /// Verifies investor accreditation (Operations Team only)
        /// </summary>
        /// <param name="investorAccreditationId">The investor accreditation ID</param>
        /// <param name="verifiedByUserId">User ID of the verifier (Operations Team member)</param>
        /// <param name="request">Verification request with approval status and notes</param>
        /// <returns>Updated accreditation response with status code 200 (OK)</returns>
        Task<ApiResponse<AccreditationResponse>> VerifyAccreditationAsync(int investorAccreditationId, string verifiedByUserId, VerifyAccreditationRequest request);

        /// <summary>
        /// Gets accreditation information by investor profile ID
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <returns>Accreditation response with status code 200 (OK) or 404 (Not Found)</returns>
        Task<ApiResponse<AccreditationResponse>> GetByInvestorProfileIdAsync(int investorProfileId);

        /// <summary>
        /// Deletes an accreditation document
        /// </summary>
        /// <param name="documentId">The accreditation document ID</param>
        /// <returns>Success status with status code 200 (OK) or 404 (Not Found)</returns>
        Task<ApiResponse<bool>> DeleteDocumentAsync(int documentId);
    }
}