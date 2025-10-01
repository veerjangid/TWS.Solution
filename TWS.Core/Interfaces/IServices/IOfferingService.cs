using TWS.Core.DTOs.Request.Offering;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Investment;

namespace TWS.Core.Interfaces.IServices
{
    /// <summary>
    /// Service interface for Offering management
    /// Handles business logic for investment offerings
    /// Reference: APIDoc.md Section 12
    /// </summary>
    public interface IOfferingService
    {
        /// <summary>
        /// Gets all offerings
        /// </summary>
        /// <returns>Collection of all offerings</returns>
        Task<IEnumerable<OfferingResponse>> GetAllOfferingsAsync();

        /// <summary>
        /// Gets all active offerings (status = Raising)
        /// </summary>
        /// <returns>Collection of active offerings</returns>
        Task<IEnumerable<OfferingResponse>> GetActiveOfferingsAsync();

        /// <summary>
        /// Gets an offering by ID
        /// </summary>
        /// <param name="id">Offering ID</param>
        /// <returns>Offering details if found, null otherwise</returns>
        Task<OfferingResponse?> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new offering
        /// </summary>
        /// <param name="request">Offering creation request</param>
        /// <param name="createdByUserId">User ID of the creator</param>
        /// <returns>Created offering details</returns>
        Task<ApiResponse<OfferingResponse>> CreateOfferingAsync(CreateOfferingRequest request, string createdByUserId);

        /// <summary>
        /// Updates an existing offering
        /// </summary>
        /// <param name="id">Offering ID to update</param>
        /// <param name="request">Offering update request</param>
        /// <param name="modifiedByUserId">User ID of the modifier</param>
        /// <returns>Updated offering details</returns>
        Task<ApiResponse<OfferingResponse>> UpdateOfferingAsync(int id, UpdateOfferingRequest request, string modifiedByUserId);

        /// <summary>
        /// Deletes an offering
        /// </summary>
        /// <param name="id">Offering ID to delete</param>
        /// <returns>Success status</returns>
        Task<ApiResponse<bool>> DeleteOfferingAsync(int id);

        /// <summary>
        /// Uploads an image for an offering
        /// </summary>
        /// <param name="offeringId">Offering ID</param>
        /// <param name="imagePath">Path to the uploaded image</param>
        /// <returns>Image path</returns>
        Task<ApiResponse<string>> UploadImageAsync(int offeringId, string imagePath);

        /// <summary>
        /// Uploads a PDF for an offering
        /// </summary>
        /// <param name="offeringId">Offering ID</param>
        /// <param name="pdfPath">Path to the uploaded PDF</param>
        /// <returns>PDF path</returns>
        Task<ApiResponse<string>> UploadPDFAsync(int offeringId, string pdfPath);

        /// <summary>
        /// Uploads an additional document for an offering
        /// </summary>
        /// <param name="offeringId">Offering ID</param>
        /// <param name="documentName">Name of the document</param>
        /// <param name="filePath">Path to the uploaded file</param>
        /// <returns>Document details</returns>
        Task<ApiResponse<OfferingDocumentResponse>> UploadDocumentAsync(int offeringId, string documentName, string filePath);

        /// <summary>
        /// Gets all documents for an offering
        /// </summary>
        /// <param name="offeringId">Offering ID</param>
        /// <returns>Collection of offering documents</returns>
        Task<ApiResponse<List<OfferingDocumentResponse>>> GetOfferingDocumentsAsync(int offeringId);
    }
}