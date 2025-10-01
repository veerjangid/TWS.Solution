using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TWS.Core.DTOs.Request.Offering;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Investment;
using TWS.Core.Enums;
using TWS.Core.Interfaces.IServices;
using TWS.Data.Context;
using TWS.Data.Entities.Portal;

namespace TWS.Infra.Services.Investment
{
    /// <summary>
    /// Service implementation for Offering management
    /// Handles business logic for investment offerings
    /// Reference: APIDoc.md Section 12
    /// </summary>
    public class OfferingService : IOfferingService
    {
        private readonly TWSDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<OfferingService> _logger;

        public OfferingService(
            TWSDbContext context,
            IMapper mapper,
            ILogger<OfferingService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all offerings
        /// </summary>
        public async Task<IEnumerable<OfferingResponse>> GetAllOfferingsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all offerings");

                var offerings = await _context.Offerings
                    .OrderByDescending(o => o.CreatedDate)
                    .ToListAsync();

                var response = _mapper.Map<IEnumerable<OfferingResponse>>(offerings);

                _logger.LogInformation("Retrieved {Count} offerings", offerings.Count);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all offerings");
                throw;
            }
        }

        /// <summary>
        /// Gets all active offerings (status = Raising)
        /// </summary>
        public async Task<IEnumerable<OfferingResponse>> GetActiveOfferingsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving active offerings");

                var offerings = await _context.Offerings
                    .Where(o => o.Status == TWS.Core.Enums.OfferingStatus.Raising)
                    .OrderByDescending(o => o.CreatedDate)
                    .ToListAsync();

                var response = _mapper.Map<IEnumerable<OfferingResponse>>(offerings);

                _logger.LogInformation("Retrieved {Count} active offerings", offerings.Count);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active offerings");
                throw;
            }
        }

        /// <summary>
        /// Gets an offering by ID
        /// </summary>
        public async Task<OfferingResponse?> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving offering {OfferingId}", id);

                var offering = await _context.Offerings
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (offering == null)
                {
                    _logger.LogWarning("Offering {OfferingId} not found", id);
                    return null;
                }

                var response = _mapper.Map<OfferingResponse>(offering);

                _logger.LogInformation("Retrieved offering {OfferingId}", id);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving offering {OfferingId}", id);
                throw;
            }
        }

        /// <summary>
        /// Creates a new offering
        /// </summary>
        public async Task<ApiResponse<OfferingResponse>> CreateOfferingAsync(CreateOfferingRequest request, string createdByUserId)
        {
            try
            {
                _logger.LogInformation("Creating new offering: {OfferingName}", request.Name);

                var offering = new Offering
                {
                    Name = request.Name,
                    Description = request.Description,
                    OfferingType = request.OfferingType,
                    TotalValue = request.TotalValue,
                    Status = (OfferingStatus)request.Status,
                    CreatedByUserId = createdByUserId,
                    CreatedDate = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Offerings.Add(offering);
                await _context.SaveChangesAsync();

                var response = _mapper.Map<OfferingResponse>(offering);

                _logger.LogInformation("Created offering with ID {OfferingId}", offering.Id);
                return ApiResponse<OfferingResponse>.SuccessResponse(response, "Offering created successfully", 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating offering");
                return ApiResponse<OfferingResponse>.ErrorResponse("Failed to create offering", 500);
            }
        }

        /// <summary>
        /// Updates an existing offering
        /// </summary>
        public async Task<ApiResponse<OfferingResponse>> UpdateOfferingAsync(int id, UpdateOfferingRequest request, string modifiedByUserId)
        {
            try
            {
                _logger.LogInformation("Updating offering {OfferingId}", id);

                var offering = await _context.Offerings.FindAsync(id);

                if (offering == null)
                {
                    _logger.LogWarning("Offering {OfferingId} not found", id);
                    return ApiResponse<OfferingResponse>.ErrorResponse("Offering not found", 404);
                }

                offering.Name = request.Name;
                offering.Description = request.Description;
                offering.OfferingType = request.OfferingType;
                offering.TotalValue = request.TotalValue;
                offering.Status = (OfferingStatus)request.Status;
                offering.ModifiedByUserId = modifiedByUserId;
                offering.LastModifiedDate = DateTime.UtcNow;
                offering.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var response = _mapper.Map<OfferingResponse>(offering);

                _logger.LogInformation("Updated offering {OfferingId}", id);
                return ApiResponse<OfferingResponse>.SuccessResponse(response, "Offering updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating offering {OfferingId}", id);
                return ApiResponse<OfferingResponse>.ErrorResponse("Failed to update offering", 500);
            }
        }

        /// <summary>
        /// Deletes an offering
        /// </summary>
        public async Task<ApiResponse<bool>> DeleteOfferingAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting offering {OfferingId}", id);

                var offering = await _context.Offerings.FindAsync(id);

                if (offering == null)
                {
                    _logger.LogWarning("Offering {OfferingId} not found", id);
                    return ApiResponse<bool>.ErrorResponse("Offering not found", 404);
                }

                _context.Offerings.Remove(offering);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Deleted offering {OfferingId}", id);
                return ApiResponse<bool>.SuccessResponse(true, "Offering deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting offering {OfferingId}", id);
                return ApiResponse<bool>.ErrorResponse("Failed to delete offering", 500);
            }
        }

        /// <summary>
        /// Uploads an image for an offering
        /// </summary>
        public async Task<ApiResponse<string>> UploadImageAsync(int offeringId, string imagePath)
        {
            try
            {
                _logger.LogInformation("Uploading image for offering {OfferingId}", offeringId);

                var offering = await _context.Offerings.FindAsync(offeringId);

                if (offering == null)
                {
                    _logger.LogWarning("Offering {OfferingId} not found", offeringId);
                    return ApiResponse<string>.ErrorResponse("Offering not found", 404);
                }

                offering.ImagePath = imagePath;
                offering.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Uploaded image for offering {OfferingId}", offeringId);
                return ApiResponse<string>.SuccessResponse(imagePath, "Image uploaded successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image for offering {OfferingId}", offeringId);
                return ApiResponse<string>.ErrorResponse("Failed to upload image", 500);
            }
        }

        /// <summary>
        /// Uploads a PDF for an offering
        /// </summary>
        public async Task<ApiResponse<string>> UploadPDFAsync(int offeringId, string pdfPath)
        {
            try
            {
                _logger.LogInformation("Uploading PDF for offering {OfferingId}", offeringId);

                var offering = await _context.Offerings.FindAsync(offeringId);

                if (offering == null)
                {
                    _logger.LogWarning("Offering {OfferingId} not found", offeringId);
                    return ApiResponse<string>.ErrorResponse("Offering not found", 404);
                }

                offering.PDFPath = pdfPath;
                offering.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Uploaded PDF for offering {OfferingId}", offeringId);
                return ApiResponse<string>.SuccessResponse(pdfPath, "PDF uploaded successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading PDF for offering {OfferingId}", offeringId);
                return ApiResponse<string>.ErrorResponse("Failed to upload PDF", 500);
            }
        }

        /// <summary>
        /// Uploads an additional document for an offering
        /// </summary>
        public async Task<ApiResponse<OfferingDocumentResponse>> UploadDocumentAsync(int offeringId, string documentName, string filePath)
        {
            try
            {
                _logger.LogInformation("Uploading document for offering {OfferingId}", offeringId);

                var offering = await _context.Offerings.FindAsync(offeringId);

                if (offering == null)
                {
                    _logger.LogWarning("Offering {OfferingId} not found", offeringId);
                    return ApiResponse<OfferingDocumentResponse>.ErrorResponse("Offering not found", 404);
                }

                var document = new OfferingDocument
                {
                    OfferingId = offeringId,
                    DocumentName = documentName,
                    FilePath = filePath,
                    UploadDate = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };

                _context.OfferingDocuments.Add(document);
                await _context.SaveChangesAsync();

                var response = _mapper.Map<OfferingDocumentResponse>(document);

                _logger.LogInformation("Uploaded document {DocumentId} for offering {OfferingId}", document.Id, offeringId);
                return ApiResponse<OfferingDocumentResponse>.SuccessResponse(response, "Document uploaded successfully", 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading document for offering {OfferingId}", offeringId);
                return ApiResponse<OfferingDocumentResponse>.ErrorResponse("Failed to upload document", 500);
            }
        }

        /// <summary>
        /// Gets all documents for an offering
        /// </summary>
        public async Task<ApiResponse<List<OfferingDocumentResponse>>> GetOfferingDocumentsAsync(int offeringId)
        {
            try
            {
                _logger.LogInformation("Retrieving documents for offering {OfferingId}", offeringId);

                var offering = await _context.Offerings.FindAsync(offeringId);

                if (offering == null)
                {
                    _logger.LogWarning("Offering {OfferingId} not found", offeringId);
                    return ApiResponse<List<OfferingDocumentResponse>>.ErrorResponse("Offering not found", 404);
                }

                var documents = await _context.OfferingDocuments
                    .Where(d => d.OfferingId == offeringId)
                    .OrderByDescending(d => d.UploadDate)
                    .ToListAsync();

                var response = _mapper.Map<List<OfferingDocumentResponse>>(documents);

                _logger.LogInformation("Retrieved {Count} documents for offering {OfferingId}", documents.Count, offeringId);
                return ApiResponse<List<OfferingDocumentResponse>>.SuccessResponse(response, "Documents retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving documents for offering {OfferingId}", offeringId);
                return ApiResponse<List<OfferingDocumentResponse>>.ErrorResponse("Failed to retrieve documents", 500);
            }
        }
    }
}