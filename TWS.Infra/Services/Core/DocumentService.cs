using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Document;
using TWS.Core.Interfaces.IServices;
using TWS.Data.Context;
using TWS.Data.Entities.Documents;

namespace TWS.Infra.Services.Core
{
    /// <summary>
    /// Service for managing document upload and retrieval operations
    /// Handles file path validation and physical file deletion
    /// Reference: APIDoc.md Section 10, BusinessRequirement.md Section 11
    /// </summary>
    public class DocumentService : IDocumentService
    {
        private readonly TWSDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<DocumentService> _logger;

        public DocumentService(
            TWSDbContext context,
            IMapper mapper,
            ILogger<DocumentService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Uploads a single document for an investor profile
        /// </summary>
        public async Task<ApiResponse<DocumentResponse>> UploadDocumentAsync(
            int investorProfileId,
            string documentName,
            string filePath,
            string fileName,
            long? fileSize,
            string? contentType)
        {
            try
            {
                _logger.LogInformation("Uploading document '{DocumentName}' for InvestorProfile {InvestorProfileId}",
                    documentName, investorProfileId);

                // Validate InvestorProfile exists
                var profileExists = await _context.InvestorProfiles
                    .AnyAsync(p => p.Id == investorProfileId);

                if (!profileExists)
                {
                    _logger.LogWarning("InvestorProfile {InvestorProfileId} not found", investorProfileId);
                    return ApiResponse<DocumentResponse>.ErrorResponse(
                        "Investor profile not found",
                        404);
                }

                // Create document entity
                var document = new InvestorDocument
                {
                    InvestorProfileId = investorProfileId,
                    DocumentName = documentName,
                    FileName = fileName,
                    FilePath = filePath,
                    FileSize = fileSize,
                    ContentType = contentType,
                    UploadDate = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };

                _context.InvestorDocuments.Add(document);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Document {DocumentId} uploaded successfully for InvestorProfile {InvestorProfileId}",
                    document.Id, investorProfileId);

                // Map to response
                var response = _mapper.Map<DocumentResponse>(document);

                return ApiResponse<DocumentResponse>.SuccessResponse(
                    response,
                    "Document uploaded successfully",
                    201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading document '{DocumentName}' for InvestorProfile {InvestorProfileId}",
                    documentName, investorProfileId);
                return ApiResponse<DocumentResponse>.ErrorResponse(
                    "An error occurred while uploading the document",
                    500);
            }
        }

        /// <summary>
        /// Uploads multiple documents for an investor profile
        /// </summary>
        public async Task<ApiResponse<List<DocumentResponse>>> UploadMultipleDocumentsAsync(
            int investorProfileId,
            List<(string documentName, string filePath, string fileName, long? fileSize, string? contentType)> documents)
        {
            try
            {
                _logger.LogInformation("Uploading {Count} documents for InvestorProfile {InvestorProfileId}",
                    documents.Count, investorProfileId);

                // Validate InvestorProfile exists
                var profileExists = await _context.InvestorProfiles
                    .AnyAsync(p => p.Id == investorProfileId);

                if (!profileExists)
                {
                    _logger.LogWarning("InvestorProfile {InvestorProfileId} not found", investorProfileId);
                    return ApiResponse<List<DocumentResponse>>.ErrorResponse(
                        "Investor profile not found",
                        404);
                }

                // Create document entities
                var documentEntities = new List<InvestorDocument>();
                foreach (var doc in documents)
                {
                    var document = new InvestorDocument
                    {
                        InvestorProfileId = investorProfileId,
                        DocumentName = doc.documentName,
                        FileName = doc.fileName,
                        FilePath = doc.filePath,
                        FileSize = doc.fileSize,
                        ContentType = doc.contentType,
                        UploadDate = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    };
                    documentEntities.Add(document);
                }

                _context.InvestorDocuments.AddRange(documentEntities);
                await _context.SaveChangesAsync();

                _logger.LogInformation("{Count} documents uploaded successfully for InvestorProfile {InvestorProfileId}",
                    documents.Count, investorProfileId);

                // Map to response
                var response = _mapper.Map<List<DocumentResponse>>(documentEntities);

                return ApiResponse<List<DocumentResponse>>.SuccessResponse(
                    response,
                    "Documents uploaded successfully",
                    201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading multiple documents for InvestorProfile {InvestorProfileId}",
                    investorProfileId);
                return ApiResponse<List<DocumentResponse>>.ErrorResponse(
                    "An error occurred while uploading the documents",
                    500);
            }
        }

        /// <summary>
        /// Gets all documents for an investor profile
        /// </summary>
        public async Task<ApiResponse<List<DocumentResponse>>> GetByInvestorProfileIdAsync(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Retrieving documents for InvestorProfile {InvestorProfileId}", investorProfileId);

                // Validate InvestorProfile exists
                var profileExists = await _context.InvestorProfiles
                    .AnyAsync(p => p.Id == investorProfileId);

                if (!profileExists)
                {
                    _logger.LogWarning("InvestorProfile {InvestorProfileId} not found", investorProfileId);
                    return ApiResponse<List<DocumentResponse>>.ErrorResponse(
                        "Investor profile not found",
                        404);
                }

                // Get documents
                var documents = await _context.InvestorDocuments
                    .Where(d => d.InvestorProfileId == investorProfileId)
                    .OrderByDescending(d => d.UploadDate)
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} documents for InvestorProfile {InvestorProfileId}",
                    documents.Count, investorProfileId);

                // Map to response
                var response = _mapper.Map<List<DocumentResponse>>(documents);

                return ApiResponse<List<DocumentResponse>>.SuccessResponse(
                    response,
                    "Documents retrieved successfully",
                    200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving documents for InvestorProfile {InvestorProfileId}",
                    investorProfileId);
                return ApiResponse<List<DocumentResponse>>.ErrorResponse(
                    "An error occurred while retrieving documents",
                    500);
            }
        }

        /// <summary>
        /// Deletes a document by ID and removes the physical file
        /// </summary>
        public async Task<ApiResponse<bool>> DeleteDocumentAsync(int documentId)
        {
            try
            {
                _logger.LogInformation("Deleting document {DocumentId}", documentId);

                // Get document
                var document = await _context.InvestorDocuments
                    .FirstOrDefaultAsync(d => d.Id == documentId);

                if (document == null)
                {
                    _logger.LogWarning("Document {DocumentId} not found", documentId);
                    return ApiResponse<bool>.ErrorResponse(
                        "Document not found",
                        404);
                }

                // Delete physical file if it exists
                try
                {
                    if (File.Exists(document.FilePath))
                    {
                        File.Delete(document.FilePath);
                        _logger.LogInformation("Physical file deleted: {FilePath}", document.FilePath);
                    }
                    else
                    {
                        _logger.LogWarning("Physical file not found: {FilePath}", document.FilePath);
                    }
                }
                catch (Exception fileEx)
                {
                    _logger.LogError(fileEx, "Error deleting physical file: {FilePath}", document.FilePath);
                    // Continue with database deletion even if file deletion fails
                }

                // Delete from database
                _context.InvestorDocuments.Remove(document);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Document {DocumentId} deleted successfully", documentId);

                return ApiResponse<bool>.SuccessResponse(
                    true,
                    "Document deleted successfully",
                    200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting document {DocumentId}", documentId);
                return ApiResponse<bool>.ErrorResponse(
                    "An error occurred while deleting the document",
                    500);
            }
        }

        /// <summary>
        /// Gets a document by ID
        /// </summary>
        public async Task<ApiResponse<DocumentResponse>> GetByIdAsync(int documentId)
        {
            try
            {
                _logger.LogInformation("Retrieving document {DocumentId}", documentId);

                // Get document
                var document = await _context.InvestorDocuments
                    .FirstOrDefaultAsync(d => d.Id == documentId);

                if (document == null)
                {
                    _logger.LogWarning("Document {DocumentId} not found", documentId);
                    return ApiResponse<DocumentResponse>.ErrorResponse(
                        "Document not found",
                        404);
                }

                _logger.LogInformation("Document {DocumentId} retrieved successfully", documentId);

                // Map to response
                var response = _mapper.Map<DocumentResponse>(document);

                return ApiResponse<DocumentResponse>.SuccessResponse(
                    response,
                    "Document retrieved successfully",
                    200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving document {DocumentId}", documentId);
                return ApiResponse<DocumentResponse>.ErrorResponse(
                    "An error occurred while retrieving the document",
                    500);
            }
        }
    }
}