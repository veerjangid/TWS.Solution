using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TWS.Core.DTOs.Request.Accreditation;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Accreditation;
using TWS.Core.Enums;
using TWS.Core.Interfaces.IServices;
using TWS.Data.Context;
using TWS.Data.Entities.Accreditation;

namespace TWS.Infra.Services.Core
{
    /// <summary>
    /// Service implementation for investor accreditation management
    /// </summary>
    public class AccreditationService : IAccreditationService
    {
        private readonly TWSDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AccreditationService> _logger;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        public AccreditationService(
            TWSDbContext context,
            IMapper mapper,
            ILogger<AccreditationService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Saves investor accreditation information (create or update)
        /// </summary>
        public async Task<ApiResponse<AccreditationResponse>> SaveAccreditationAsync(SaveAccreditationRequest request)
        {
            try
            {
                _logger.LogInformation("Saving accreditation for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);

                // Validate InvestorProfile exists
                var investorProfileExists = await _context.InvestorProfiles
                    .AnyAsync(ip => ip.Id == request.InvestorProfileId);

                if (!investorProfileExists)
                {
                    _logger.LogWarning("InvestorProfile with ID {InvestorProfileId} not found", request.InvestorProfileId);
                    return ApiResponse<AccreditationResponse>.ErrorResponse(
                        $"Investor profile with ID {request.InvestorProfileId} not found",
                        404);
                }

                // Validate AccreditationType
                if (!Enum.IsDefined(typeof(AccreditationType), request.AccreditationType))
                {
                    _logger.LogWarning("Invalid AccreditationType: {AccreditationType}", request.AccreditationType);
                    return ApiResponse<AccreditationResponse>.ErrorResponse(
                        "Invalid accreditation type. Must be between 1 and 6.",
                        400);
                }

                // Validate LicenseNumber and StateLicenseHeld for license-based types (Series7, Series65, Series82)
                var licenseBasedTypes = new[]
                {
                    (int)AccreditationType.Series7,
                    (int)AccreditationType.Series65,
                    (int)AccreditationType.Series82
                };

                if (licenseBasedTypes.Contains(request.AccreditationType))
                {
                    if (string.IsNullOrWhiteSpace(request.LicenseNumber))
                    {
                        _logger.LogWarning("LicenseNumber is required for license-based AccreditationType");
                        return ApiResponse<AccreditationResponse>.ErrorResponse(
                            "License Number is required for license-based accreditation types",
                            400);
                    }

                    if (string.IsNullOrWhiteSpace(request.StateLicenseHeld))
                    {
                        _logger.LogWarning("StateLicenseHeld is required for license-based AccreditationType");
                        return ApiResponse<AccreditationResponse>.ErrorResponse(
                            "State License Held is required for license-based accreditation types",
                            400);
                    }
                }

                // Check if accreditation already exists (one-to-one relationship)
                var existingAccreditation = await _context.InvestorAccreditations
                    .Include(ia => ia.AccreditationDocuments)
                    .Include(ia => ia.VerifiedByUser)
                    .FirstOrDefaultAsync(ia => ia.InvestorProfileId == request.InvestorProfileId);

                bool isCreating = existingAccreditation == null;

                if (isCreating)
                {
                    // Create new accreditation
                    var newAccreditation = _mapper.Map<InvestorAccreditation>(request);
                    newAccreditation.CreatedAt = DateTime.UtcNow;
                    newAccreditation.IsVerified = false;

                    _context.InvestorAccreditations.Add(newAccreditation);
                    await _context.SaveChangesAsync();

                    // Load the created entity with related data
                    existingAccreditation = await _context.InvestorAccreditations
                        .Include(ia => ia.AccreditationDocuments)
                        .Include(ia => ia.VerifiedByUser)
                        .FirstOrDefaultAsync(ia => ia.Id == newAccreditation.Id);

                    if (existingAccreditation == null)
                    {
                        _logger.LogError("Failed to reload created accreditation with ID: {AccreditationId}", newAccreditation.Id);
                        return ApiResponse<AccreditationResponse>.ErrorResponse(
                            "An error occurred while creating accreditation",
                            500);
                    }

                    _logger.LogInformation("Accreditation created successfully with ID: {AccreditationId}", newAccreditation.Id);

                    var createdResponse = _mapper.Map<AccreditationResponse>(existingAccreditation);
                    return ApiResponse<AccreditationResponse>.SuccessResponse(
                        createdResponse,
                        "Accreditation created successfully",
                        201);
                }
                else
                {
                    // Update existing accreditation
                    existingAccreditation.AccreditationType = (AccreditationType)request.AccreditationType;
                    existingAccreditation.LicenseNumber = request.LicenseNumber;
                    existingAccreditation.StateLicenseHeld = request.StateLicenseHeld;
                    existingAccreditation.UpdatedAt = DateTime.UtcNow;

                    // Reset verification if accreditation type changed
                    existingAccreditation.IsVerified = false;
                    existingAccreditation.VerificationDate = null;
                    existingAccreditation.VerifiedByUserId = null;

                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Accreditation updated successfully with ID: {AccreditationId}", existingAccreditation.Id);

                    var updatedResponse = _mapper.Map<AccreditationResponse>(existingAccreditation);
                    return ApiResponse<AccreditationResponse>.SuccessResponse(
                        updatedResponse,
                        "Accreditation updated successfully",
                        200);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving accreditation for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);
                return ApiResponse<AccreditationResponse>.ErrorResponse(
                    "An error occurred while saving accreditation",
                    500);
            }
        }

        /// <summary>
        /// Uploads an accreditation document for an investor
        /// </summary>
        public async Task<ApiResponse<AccreditationDocumentResponse>> UploadDocumentAsync(
            int investorAccreditationId,
            string documentType,
            string filePath)
        {
            try
            {
                _logger.LogInformation("Uploading document for InvestorAccreditationId: {InvestorAccreditationId}", investorAccreditationId);

                // Validate InvestorAccreditation exists
                var accreditationExists = await _context.InvestorAccreditations
                    .AnyAsync(ia => ia.Id == investorAccreditationId);

                if (!accreditationExists)
                {
                    _logger.LogWarning("InvestorAccreditation with ID {InvestorAccreditationId} not found", investorAccreditationId);
                    return ApiResponse<AccreditationDocumentResponse>.ErrorResponse(
                        $"Investor accreditation with ID {investorAccreditationId} not found",
                        404);
                }

                // Create document entity
                var document = new AccreditationDocument
                {
                    InvestorAccreditationId = investorAccreditationId,
                    DocumentType = documentType,
                    DocumentPath = filePath,
                    UploadDate = DateTime.UtcNow
                };

                _context.AccreditationDocuments.Add(document);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Document uploaded successfully with ID: {DocumentId}", document.Id);

                var response = _mapper.Map<AccreditationDocumentResponse>(document);
                return ApiResponse<AccreditationDocumentResponse>.SuccessResponse(
                    response,
                    "Document uploaded successfully",
                    201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading document for InvestorAccreditationId: {InvestorAccreditationId}", investorAccreditationId);
                return ApiResponse<AccreditationDocumentResponse>.ErrorResponse(
                    "An error occurred while uploading document",
                    500);
            }
        }

        /// <summary>
        /// Verifies investor accreditation (Operations Team only)
        /// </summary>
        public async Task<ApiResponse<AccreditationResponse>> VerifyAccreditationAsync(
            int investorAccreditationId,
            string verifiedByUserId,
            VerifyAccreditationRequest request)
        {
            try
            {
                _logger.LogInformation("Verifying accreditation ID: {InvestorAccreditationId} by User: {UserId}",
                    investorAccreditationId, verifiedByUserId);

                // Validate InvestorAccreditation exists
                var accreditation = await _context.InvestorAccreditations
                    .Include(ia => ia.AccreditationDocuments)
                    .Include(ia => ia.VerifiedByUser)
                    .FirstOrDefaultAsync(ia => ia.Id == investorAccreditationId);

                if (accreditation == null)
                {
                    _logger.LogWarning("InvestorAccreditation with ID {InvestorAccreditationId} not found", investorAccreditationId);
                    return ApiResponse<AccreditationResponse>.ErrorResponse(
                        $"Investor accreditation with ID {investorAccreditationId} not found",
                        404);
                }

                // Update verification status
                if (request.IsApproved)
                {
                    accreditation.IsVerified = true;
                    accreditation.VerificationDate = DateTime.UtcNow;
                    accreditation.VerifiedByUserId = verifiedByUserId;
                    _logger.LogInformation("Accreditation approved for ID: {InvestorAccreditationId}", investorAccreditationId);
                }
                else
                {
                    accreditation.IsVerified = false;
                    accreditation.VerificationDate = null;
                    accreditation.VerifiedByUserId = null;
                    _logger.LogInformation("Accreditation rejected for ID: {InvestorAccreditationId}", investorAccreditationId);
                }

                accreditation.Notes = request.Notes;
                accreditation.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Reload to get VerifiedByUser name
                accreditation = await _context.InvestorAccreditations
                    .Include(ia => ia.AccreditationDocuments)
                    .Include(ia => ia.VerifiedByUser)
                    .FirstOrDefaultAsync(ia => ia.Id == investorAccreditationId);

                _logger.LogInformation("Accreditation verification updated successfully for ID: {InvestorAccreditationId}", investorAccreditationId);

                var response = _mapper.Map<AccreditationResponse>(accreditation);
                return ApiResponse<AccreditationResponse>.SuccessResponse(
                    response,
                    request.IsApproved ? "Accreditation verified successfully" : "Accreditation verification rejected",
                    200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying accreditation ID: {InvestorAccreditationId}", investorAccreditationId);
                return ApiResponse<AccreditationResponse>.ErrorResponse(
                    "An error occurred while verifying accreditation",
                    500);
            }
        }

        /// <summary>
        /// Gets accreditation information by investor profile ID
        /// </summary>
        public async Task<ApiResponse<AccreditationResponse>> GetByInvestorProfileIdAsync(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Getting accreditation for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                // Load accreditation with all related data
                var accreditation = await _context.InvestorAccreditations
                    .Include(ia => ia.AccreditationDocuments)
                    .Include(ia => ia.VerifiedByUser)
                    .FirstOrDefaultAsync(ia => ia.InvestorProfileId == investorProfileId);

                if (accreditation == null)
                {
                    _logger.LogWarning("Accreditation not found for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                    return ApiResponse<AccreditationResponse>.ErrorResponse(
                        $"Accreditation not found for investor profile ID {investorProfileId}",
                        404);
                }

                var response = _mapper.Map<AccreditationResponse>(accreditation);
                return ApiResponse<AccreditationResponse>.SuccessResponse(
                    response,
                    "Accreditation retrieved successfully",
                    200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting accreditation for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return ApiResponse<AccreditationResponse>.ErrorResponse(
                    "An error occurred while retrieving accreditation",
                    500);
            }
        }

        /// <summary>
        /// Deletes an accreditation document
        /// </summary>
        public async Task<ApiResponse<bool>> DeleteDocumentAsync(int documentId)
        {
            try
            {
                _logger.LogInformation("Deleting document with ID: {DocumentId}", documentId);

                // Validate document exists
                var document = await _context.AccreditationDocuments
                    .FirstOrDefaultAsync(d => d.Id == documentId);

                if (document == null)
                {
                    _logger.LogWarning("Document with ID {DocumentId} not found", documentId);
                    return ApiResponse<bool>.ErrorResponse(
                        $"Document with ID {documentId} not found",
                        404);
                }

                _context.AccreditationDocuments.Remove(document);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Document deleted successfully with ID: {DocumentId}", documentId);

                return ApiResponse<bool>.SuccessResponse(
                    true,
                    "Document deleted successfully",
                    200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting document with ID: {DocumentId}", documentId);
                return ApiResponse<bool>.ErrorResponse(
                    "An error occurred while deleting document",
                    500);
            }
        }
    }
}