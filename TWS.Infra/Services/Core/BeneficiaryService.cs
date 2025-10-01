using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TWS.Core.DTOs.Request.Beneficiary;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Beneficiary;
using TWS.Core.Enums;
using TWS.Core.Interfaces.IServices;
using TWS.Data.Context;
using TWS.Data.Entities.Beneficiaries;

namespace TWS.Infra.Services.Core
{
    /// <summary>
    /// Service for managing beneficiary operations
    /// Implements percentage validation per beneficiary type
    /// </summary>
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly TWSDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BeneficiaryService> _logger;

        public BeneficiaryService(
            TWSDbContext context,
            IMapper mapper,
            ILogger<BeneficiaryService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Adds a single beneficiary to an investor profile
        /// Validates total percentage constraint per beneficiary type
        /// </summary>
        public async Task<ApiResponse<BeneficiaryResponse>> AddBeneficiaryAsync(AddBeneficiaryRequest request)
        {
            try
            {
                _logger.LogInformation("Adding beneficiary for InvestorProfile {InvestorProfileId}", request.InvestorProfileId);

                // Validate InvestorProfile exists
                var profileExists = await _context.InvestorProfiles
                    .AnyAsync(p => p.Id == request.InvestorProfileId);

                if (!profileExists)
                {
                    _logger.LogWarning("InvestorProfile {InvestorProfileId} not found", request.InvestorProfileId);
                    return ApiResponse<BeneficiaryResponse>.ErrorResponse(
                        "Investor profile not found",
                        404);
                }

                // Validate BeneficiaryType
                if (request.BeneficiaryType != (int)BeneficiaryType.Primary &&
                    request.BeneficiaryType != (int)BeneficiaryType.Contingent)
                {
                    _logger.LogWarning("Invalid BeneficiaryType {BeneficiaryType}", request.BeneficiaryType);
                    return ApiResponse<BeneficiaryResponse>.ErrorResponse(
                        "Invalid beneficiary type. Must be 1 (Primary) or 2 (Contingent)",
                        400);
                }

                // Get existing beneficiaries of same type
                var existingBeneficiaries = await _context.Beneficiaries
                    .Where(b => b.InvestorProfileId == request.InvestorProfileId &&
                               (int)b.BeneficiaryType == request.BeneficiaryType)
                    .ToListAsync();

                // Calculate total percentage
                var currentTotal = existingBeneficiaries.Sum(b => b.PercentageOfBenefit);
                var newTotal = currentTotal + request.PercentageOfBenefit;

                if (newTotal > 100)
                {
                    var typeName = ((BeneficiaryType)request.BeneficiaryType).ToString();
                    _logger.LogWarning(
                        "Total percentage exceeds 100% for {TypeName} beneficiaries. Current: {Current}, New: {New}",
                        typeName, currentTotal, request.PercentageOfBenefit);
                    return ApiResponse<BeneficiaryResponse>.ErrorResponse(
                        $"Total percentage for {typeName} beneficiaries cannot exceed 100%. Current total: {currentTotal}%, attempting to add: {request.PercentageOfBenefit}%",
                        400);
                }

                // Create beneficiary
                var beneficiary = _mapper.Map<Beneficiary>(request);
                beneficiary.CreatedAt = DateTime.UtcNow;

                _context.Beneficiaries.Add(beneficiary);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Beneficiary {BeneficiaryId} created successfully", beneficiary.Id);

                // Map to response
                var response = _mapper.Map<BeneficiaryResponse>(beneficiary);

                return ApiResponse<BeneficiaryResponse>.SuccessResponse(
                    response,
                    "Beneficiary added successfully",
                    201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding beneficiary for InvestorProfile {InvestorProfileId}", request.InvestorProfileId);
                return ApiResponse<BeneficiaryResponse>.ErrorResponse(
                    "An error occurred while adding the beneficiary",
                    500);
            }
        }

        /// <summary>
        /// Adds multiple beneficiaries to an investor profile
        /// Replaces existing beneficiaries of the same types
        /// Validates that total percentage equals 100% per type
        /// </summary>
        public async Task<ApiResponse<List<BeneficiaryResponse>>> AddMultipleBeneficiariesAsync(AddMultipleBeneficiariesRequest request)
        {
            try
            {
                _logger.LogInformation("Adding multiple beneficiaries for InvestorProfile {InvestorProfileId}", request.InvestorProfileId);

                // Validate InvestorProfile exists
                var profileExists = await _context.InvestorProfiles
                    .AnyAsync(p => p.Id == request.InvestorProfileId);

                if (!profileExists)
                {
                    _logger.LogWarning("InvestorProfile {InvestorProfileId} not found", request.InvestorProfileId);
                    return ApiResponse<List<BeneficiaryResponse>>.ErrorResponse(
                        "Investor profile not found",
                        404);
                }

                // Group beneficiaries by type
                var beneficiariesByType = request.Beneficiaries.GroupBy(b => b.BeneficiaryType).ToList();

                // Validate percentage totals for each type
                foreach (var group in beneficiariesByType)
                {
                    var beneficiaryType = group.Key;
                    var total = group.Sum(b => b.PercentageOfBenefit);

                    if (total != 100)
                    {
                        var typeName = ((BeneficiaryType)beneficiaryType).ToString();
                        _logger.LogWarning(
                            "Total percentage for {TypeName} beneficiaries is {Total}%, must equal 100%",
                            typeName, total);
                        return ApiResponse<List<BeneficiaryResponse>>.ErrorResponse(
                            $"Total percentage for {typeName} beneficiaries must equal 100%. Current total: {total}%",
                            400);
                    }
                }

                // Get types being replaced
                var typesToReplace = beneficiariesByType.Select(g => g.Key).ToList();

                // Delete existing beneficiaries of those types
                var existingBeneficiaries = await _context.Beneficiaries
                    .Where(b => b.InvestorProfileId == request.InvestorProfileId &&
                               typesToReplace.Contains((int)b.BeneficiaryType))
                    .ToListAsync();

                if (existingBeneficiaries.Any())
                {
                    _context.Beneficiaries.RemoveRange(existingBeneficiaries);
                    _logger.LogInformation(
                        "Removed {Count} existing beneficiaries for InvestorProfile {InvestorProfileId}",
                        existingBeneficiaries.Count,
                        request.InvestorProfileId);
                }

                // Add new beneficiaries
                var newBeneficiaries = new List<Beneficiary>();
                foreach (var item in request.Beneficiaries)
                {
                    var beneficiary = _mapper.Map<Beneficiary>(item);
                    beneficiary.InvestorProfileId = request.InvestorProfileId;
                    beneficiary.CreatedAt = DateTime.UtcNow;
                    newBeneficiaries.Add(beneficiary);
                }

                _context.Beneficiaries.AddRange(newBeneficiaries);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Added {Count} beneficiaries for InvestorProfile {InvestorProfileId}",
                    newBeneficiaries.Count,
                    request.InvestorProfileId);

                // Map to response
                var responses = _mapper.Map<List<BeneficiaryResponse>>(newBeneficiaries);

                return ApiResponse<List<BeneficiaryResponse>>.SuccessResponse(
                    responses,
                    "Beneficiaries added successfully",
                    201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding multiple beneficiaries for InvestorProfile {InvestorProfileId}", request.InvestorProfileId);
                return ApiResponse<List<BeneficiaryResponse>>.ErrorResponse(
                    "An error occurred while adding beneficiaries",
                    500);
            }
        }

        /// <summary>
        /// Updates an existing beneficiary
        /// Validates percentage constraint with other beneficiaries of same type
        /// </summary>
        public async Task<ApiResponse<BeneficiaryResponse>> UpdateBeneficiaryAsync(int id, UpdateBeneficiaryRequest request)
        {
            try
            {
                _logger.LogInformation("Updating beneficiary {BeneficiaryId}", id);

                // Validate beneficiary exists
                var beneficiary = await _context.Beneficiaries.FindAsync(id);

                if (beneficiary == null)
                {
                    _logger.LogWarning("Beneficiary {BeneficiaryId} not found", id);
                    return ApiResponse<BeneficiaryResponse>.ErrorResponse(
                        "Beneficiary not found",
                        404);
                }

                // Get other beneficiaries of same type (exclude current)
                var otherBeneficiaries = await _context.Beneficiaries
                    .Where(b => b.InvestorProfileId == beneficiary.InvestorProfileId &&
                               b.BeneficiaryType == beneficiary.BeneficiaryType &&
                               b.Id != id)
                    .ToListAsync();

                // Calculate new total percentage
                var otherTotal = otherBeneficiaries.Sum(b => b.PercentageOfBenefit);
                var newTotal = otherTotal + request.PercentageOfBenefit;

                if (newTotal > 100)
                {
                    var typeName = ((BeneficiaryType)beneficiary.BeneficiaryType).ToString();
                    _logger.LogWarning(
                        "Total percentage exceeds 100% for {TypeName} beneficiaries. Other total: {OtherTotal}, New: {New}",
                        typeName, otherTotal, request.PercentageOfBenefit);
                    return ApiResponse<BeneficiaryResponse>.ErrorResponse(
                        $"Total percentage for {typeName} beneficiaries cannot exceed 100%. Other beneficiaries total: {otherTotal}%, attempting to set: {request.PercentageOfBenefit}%",
                        400);
                }

                // Update beneficiary
                _mapper.Map(request, beneficiary);
                beneficiary.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Beneficiary {BeneficiaryId} updated successfully", id);

                // Map to response
                var response = _mapper.Map<BeneficiaryResponse>(beneficiary);

                return ApiResponse<BeneficiaryResponse>.SuccessResponse(
                    response,
                    "Beneficiary updated successfully",
                    200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating beneficiary {BeneficiaryId}", id);
                return ApiResponse<BeneficiaryResponse>.ErrorResponse(
                    "An error occurred while updating the beneficiary",
                    500);
            }
        }

        /// <summary>
        /// Deletes a beneficiary by ID
        /// Logs warning about percentage recalculation
        /// </summary>
        public async Task<ApiResponse<bool>> DeleteBeneficiaryAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting beneficiary {BeneficiaryId}", id);

                // Validate beneficiary exists
                var beneficiary = await _context.Beneficiaries.FindAsync(id);

                if (beneficiary == null)
                {
                    _logger.LogWarning("Beneficiary {BeneficiaryId} not found", id);
                    return ApiResponse<bool>.ErrorResponse(
                        "Beneficiary not found",
                        404);
                }

                var typeName = ((BeneficiaryType)beneficiary.BeneficiaryType).ToString();

                _context.Beneficiaries.Remove(beneficiary);
                await _context.SaveChangesAsync();

                _logger.LogWarning(
                    "Beneficiary {BeneficiaryId} deleted. Note: Total percentage for {TypeName} beneficiaries may need recalculation",
                    id,
                    typeName);

                return ApiResponse<bool>.SuccessResponse(
                    true,
                    "Beneficiary deleted successfully",
                    200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting beneficiary {BeneficiaryId}", id);
                return ApiResponse<bool>.ErrorResponse(
                    "An error occurred while deleting the beneficiary",
                    500);
            }
        }

        /// <summary>
        /// Gets all beneficiaries for an investor profile
        /// Ordered by Type, then PercentageOfBenefit descending
        /// </summary>
        public async Task<ApiResponse<List<BeneficiaryResponse>>> GetByInvestorProfileIdAsync(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Getting beneficiaries for InvestorProfile {InvestorProfileId}", investorProfileId);

                var beneficiaries = await _context.Beneficiaries
                    .Where(b => b.InvestorProfileId == investorProfileId)
                    .OrderBy(b => b.BeneficiaryType)
                    .ThenByDescending(b => b.PercentageOfBenefit)
                    .ToListAsync();

                _logger.LogInformation(
                    "Found {Count} beneficiaries for InvestorProfile {InvestorProfileId}",
                    beneficiaries.Count,
                    investorProfileId);

                var responses = _mapper.Map<List<BeneficiaryResponse>>(beneficiaries);

                return ApiResponse<List<BeneficiaryResponse>>.SuccessResponse(
                    responses,
                    "Beneficiaries retrieved successfully",
                    200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting beneficiaries for InvestorProfile {InvestorProfileId}", investorProfileId);
                return ApiResponse<List<BeneficiaryResponse>>.ErrorResponse(
                    "An error occurred while retrieving beneficiaries",
                    500);
            }
        }

        /// <summary>
        /// Gets beneficiaries grouped by type (Primary and Contingent) with total percentages
        /// </summary>
        public async Task<ApiResponse<BeneficiariesGroupedResponse>> GetGroupedByInvestorProfileIdAsync(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Getting grouped beneficiaries for InvestorProfile {InvestorProfileId}", investorProfileId);

                var beneficiaries = await _context.Beneficiaries
                    .Where(b => b.InvestorProfileId == investorProfileId)
                    .OrderBy(b => b.BeneficiaryType)
                    .ThenByDescending(b => b.PercentageOfBenefit)
                    .ToListAsync();

                // Group by type
                var primaryBeneficiaries = beneficiaries
                    .Where(b => b.BeneficiaryType == BeneficiaryType.Primary)
                    .ToList();

                var contingentBeneficiaries = beneficiaries
                    .Where(b => b.BeneficiaryType == BeneficiaryType.Contingent)
                    .ToList();

                // Calculate totals
                var primaryTotal = primaryBeneficiaries.Sum(b => b.PercentageOfBenefit);
                var contingentTotal = contingentBeneficiaries.Sum(b => b.PercentageOfBenefit);

                // Map to response
                var response = new BeneficiariesGroupedResponse
                {
                    PrimaryBeneficiaries = _mapper.Map<List<BeneficiaryResponse>>(primaryBeneficiaries),
                    PrimaryTotalPercentage = primaryTotal,
                    ContingentBeneficiaries = _mapper.Map<List<BeneficiaryResponse>>(contingentBeneficiaries),
                    ContingentTotalPercentage = contingentTotal
                };

                _logger.LogInformation(
                    "Found {PrimaryCount} primary and {ContingentCount} contingent beneficiaries for InvestorProfile {InvestorProfileId}",
                    primaryBeneficiaries.Count,
                    contingentBeneficiaries.Count,
                    investorProfileId);

                return ApiResponse<BeneficiariesGroupedResponse>.SuccessResponse(
                    response,
                    "Grouped beneficiaries retrieved successfully",
                    200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting grouped beneficiaries for InvestorProfile {InvestorProfileId}", investorProfileId);
                return ApiResponse<BeneficiariesGroupedResponse>.ErrorResponse(
                    "An error occurred while retrieving grouped beneficiaries",
                    500);
            }
        }
    }
}