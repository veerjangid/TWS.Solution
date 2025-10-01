using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Document;

namespace TWS.Core.Interfaces.IServices
{
    /// <summary>
    /// Service interface for document management operations
    /// Reference: APIDoc.md Section 10, BusinessRequirement.md Section 11
    /// </summary>
    public interface IDocumentService
    {
        /// <summary>
        /// Uploads a single document for an investor profile
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <param name="documentName">Display name for the document</param>
        /// <param name="filePath">Full path where the file is stored</param>
        /// <param name="fileName">Original file name</param>
        /// <param name="fileSize">File size in bytes</param>
        /// <param name="contentType">MIME type of the file</param>
        /// <returns>ApiResponse with DocumentResponse</returns>
        Task<ApiResponse<DocumentResponse>> UploadDocumentAsync(
            int investorProfileId,
            string documentName,
            string filePath,
            string fileName,
            long? fileSize,
            string? contentType);

        /// <summary>
        /// Uploads multiple documents for an investor profile
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <param name="documents">List of tuples containing document info (name, path, fileName, fileSize, contentType)</param>
        /// <returns>ApiResponse with list of DocumentResponse</returns>
        Task<ApiResponse<List<DocumentResponse>>> UploadMultipleDocumentsAsync(
            int investorProfileId,
            List<(string documentName, string filePath, string fileName, long? fileSize, string? contentType)> documents);

        /// <summary>
        /// Gets all documents for an investor profile
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <returns>ApiResponse with list of DocumentResponse</returns>
        Task<ApiResponse<List<DocumentResponse>>> GetByInvestorProfileIdAsync(int investorProfileId);

        /// <summary>
        /// Deletes a document by ID and removes the physical file
        /// </summary>
        /// <param name="documentId">The document ID</param>
        /// <returns>ApiResponse with boolean indicating success</returns>
        Task<ApiResponse<bool>> DeleteDocumentAsync(int documentId);

        /// <summary>
        /// Gets a document by ID
        /// </summary>
        /// <param name="documentId">The document ID</param>
        /// <returns>ApiResponse with DocumentResponse</returns>
        Task<ApiResponse<DocumentResponse>> GetByIdAsync(int documentId);
    }
}