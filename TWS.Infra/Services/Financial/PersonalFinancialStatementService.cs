using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Net;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.PersonalFinancialStatement;
using TWS.Core.Interfaces.IRepositories;
using TWS.Core.Interfaces.IServices;
using TWS.Data.Entities.Financial;

namespace TWS.Infra.Services.Financial
{
    /// <summary>
    /// Service implementation for Personal Financial Statement operations.
    /// Handles business logic for uploading, retrieving, and deleting PFS documents.
    /// Reference: APIDoc.md Section 8, BusinessRequirement.md Section 9
    /// </summary>
    public class PersonalFinancialStatementService : IPersonalFinancialStatementService
    {
        private readonly IPersonalFinancialStatementRepository _pfsRepository;
        private readonly IInvestorProfileRepository _investorProfileRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PersonalFinancialStatementService> _logger;

        public PersonalFinancialStatementService(
            IPersonalFinancialStatementRepository pfsRepository,
            IInvestorProfileRepository investorProfileRepository,
            IMapper mapper,
            ILogger<PersonalFinancialStatementService> logger)
        {
            _pfsRepository = pfsRepository ?? throw new ArgumentNullException(nameof(pfsRepository));
            _investorProfileRepository = investorProfileRepository ?? throw new ArgumentNullException(nameof(investorProfileRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Uploads a new Personal Financial Statement for an investor profile.
        /// Replaces existing PFS if one exists (one-to-one relationship).
        /// </summary>
        public async Task<ApiResponse<PFSResponse>> UploadPFSAsync(int investorProfileId, string filePath)
        {
            try
            {
                _logger.LogInformation("Uploading PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                // Validate that the investor profile exists
                var investorProfile = await _investorProfileRepository.GetByIdAsync(investorProfileId);
                if (investorProfile == null)
                {
                    _logger.LogWarning("InvestorProfile not found for Id: {InvestorProfileId}", investorProfileId);
                    return ApiResponse<PFSResponse>.ErrorResponse(
                        "Investor profile not found",
                        (int)HttpStatusCode.NotFound
                    );
                }

                // Check if PFS already exists for this investor profile
                var existingPFS = await _pfsRepository.GetByInvestorProfileIdAsync(investorProfileId);

                PersonalFinancialStatement pfsEntity;

                if (existingPFS != null)
                {
                    // Replace existing PFS
                    _logger.LogInformation("Replacing existing PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                    var existingPFSTyped = existingPFS as PersonalFinancialStatement;
                    if (existingPFSTyped == null)
                    {
                        _logger.LogError("Failed to cast existing PFS to PersonalFinancialStatement");
                        return ApiResponse<PFSResponse>.ErrorResponse(
                            "Internal error processing existing PFS",
                            (int)HttpStatusCode.InternalServerError
                        );
                    }

                    // Delete old file if it exists
                    if (File.Exists(existingPFSTyped.FilePath))
                    {
                        try
                        {
                            File.Delete(existingPFSTyped.FilePath);
                            _logger.LogInformation("Deleted old PFS file: {FilePath}", existingPFSTyped.FilePath);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to delete old PFS file: {FilePath}", existingPFSTyped.FilePath);
                        }
                    }

                    // Update existing record
                    existingPFSTyped.FilePath = filePath;
                    existingPFSTyped.LastModifiedDate = DateTime.UtcNow;
                    existingPFSTyped.UpdatedAt = DateTime.UtcNow;

                    await _pfsRepository.UpdateAsync(existingPFSTyped);
                    pfsEntity = existingPFSTyped;
                }
                else
                {
                    // Create new PFS record
                    _logger.LogInformation("Creating new PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                    pfsEntity = new PersonalFinancialStatement
                    {
                        InvestorProfileId = investorProfileId,
                        FilePath = filePath,
                        UploadDate = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _pfsRepository.AddAsync(pfsEntity);
                }

                // Save changes
                var saved = await _pfsRepository.SaveChangesAsync();
                if (!saved)
                {
                    _logger.LogError("Failed to save PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                    return ApiResponse<PFSResponse>.ErrorResponse(
                        "Failed to save Personal Financial Statement",
                        (int)HttpStatusCode.InternalServerError
                    );
                }

                var response = _mapper.Map<PFSResponse>(pfsEntity);

                _logger.LogInformation("Successfully uploaded PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                return ApiResponse<PFSResponse>.SuccessResponse(
                    response,
                    "Personal Financial Statement uploaded successfully",
                    (int)HttpStatusCode.Created
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return ApiResponse<PFSResponse>.ErrorResponse(
                    "An error occurred while uploading the Personal Financial Statement",
                    (int)HttpStatusCode.InternalServerError
                );
            }
        }

        /// <summary>
        /// Retrieves the Personal Financial Statement for a specific investor profile.
        /// </summary>
        public async Task<ApiResponse<PFSResponse>> GetByInvestorProfileIdAsync(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Retrieving PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                var pfsEntity = await _pfsRepository.GetByInvestorProfileIdAsync(investorProfileId);

                if (pfsEntity == null)
                {
                    _logger.LogInformation("No PFS found for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                    return ApiResponse<PFSResponse>.ErrorResponse(
                        "Personal Financial Statement not found",
                        (int)HttpStatusCode.NotFound
                    );
                }

                var pfsTyped = pfsEntity as PersonalFinancialStatement;
                if (pfsTyped == null)
                {
                    _logger.LogError("Failed to cast PFS to PersonalFinancialStatement");
                    return ApiResponse<PFSResponse>.ErrorResponse(
                        "Internal error processing PFS",
                        (int)HttpStatusCode.InternalServerError
                    );
                }

                var response = _mapper.Map<PFSResponse>(pfsTyped);

                _logger.LogInformation("Successfully retrieved PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                return ApiResponse<PFSResponse>.SuccessResponse(
                    response,
                    "Personal Financial Statement retrieved successfully",
                    (int)HttpStatusCode.OK
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return ApiResponse<PFSResponse>.ErrorResponse(
                    "An error occurred while retrieving the Personal Financial Statement",
                    (int)HttpStatusCode.InternalServerError
                );
            }
        }

        /// <summary>
        /// Deletes the Personal Financial Statement for a specific investor profile.
        /// Also removes the physical file from storage.
        /// </summary>
        public async Task<ApiResponse<bool>> DeletePFSAsync(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Deleting PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                var pfsEntity = await _pfsRepository.GetByInvestorProfileIdAsync(investorProfileId);

                if (pfsEntity == null)
                {
                    _logger.LogWarning("No PFS found to delete for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                    return ApiResponse<bool>.ErrorResponse(
                        "Personal Financial Statement not found",
                        (int)HttpStatusCode.NotFound
                    );
                }

                var pfsTyped = pfsEntity as PersonalFinancialStatement;
                if (pfsTyped == null)
                {
                    _logger.LogError("Failed to cast PFS to PersonalFinancialStatement");
                    return ApiResponse<bool>.ErrorResponse(
                        "Internal error processing PFS",
                        (int)HttpStatusCode.InternalServerError
                    );
                }

                // Delete physical file
                if (File.Exists(pfsTyped.FilePath))
                {
                    try
                    {
                        File.Delete(pfsTyped.FilePath);
                        _logger.LogInformation("Deleted PFS file: {FilePath}", pfsTyped.FilePath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to delete PFS file: {FilePath}", pfsTyped.FilePath);
                    }
                }

                // Delete database record
                await _pfsRepository.DeleteAsync(pfsTyped);
                var saved = await _pfsRepository.SaveChangesAsync();

                if (!saved)
                {
                    _logger.LogError("Failed to delete PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                    return ApiResponse<bool>.ErrorResponse(
                        "Failed to delete Personal Financial Statement",
                        (int)HttpStatusCode.InternalServerError
                    );
                }

                _logger.LogInformation("Successfully deleted PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                return ApiResponse<bool>.SuccessResponse(
                    true,
                    "Personal Financial Statement deleted successfully",
                    (int)HttpStatusCode.OK
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting PFS for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return ApiResponse<bool>.ErrorResponse(
                    "An error occurred while deleting the Personal Financial Statement",
                    (int)HttpStatusCode.InternalServerError
                );
            }
        }
    }
}