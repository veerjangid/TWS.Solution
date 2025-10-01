using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TWS.Core.DTOs.Request.PrimaryInvestorInfo;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.PrimaryInvestorInfo;
using TWS.Core.Enums;
using TWS.Core.Interfaces.IServices;
using TWS.Data.Context;
using TWS.Data.Entities.PrimaryInvestorInfo;

namespace TWS.Infra.Services.Core
{
    /// <summary>
    /// Service implementation for Primary Investor Info business logic
    /// </summary>
    public class PrimaryInvestorInfoService : IPrimaryInvestorInfoService
    {
        private readonly TWSDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PrimaryInvestorInfoService> _logger;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        public PrimaryInvestorInfoService(
            TWSDbContext context,
            IMapper mapper,
            ILogger<PrimaryInvestorInfoService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Saves or updates the primary investor information
        /// </summary>
        public async Task<ApiResponse<PrimaryInvestorInfoResponse>> SavePrimaryInvestorInfoAsync(SavePrimaryInvestorInfoRequest request)
        {
            try
            {
                _logger.LogInformation("Saving Primary Investor Info for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);

                // Validate InvestorProfile exists
                var investorProfile = await _context.InvestorProfiles
                    .FirstOrDefaultAsync(ip => ip.Id == request.InvestorProfileId);

                if (investorProfile == null)
                {
                    _logger.LogWarning("InvestorProfile not found with ID: {InvestorProfileId}", request.InvestorProfileId);
                    return ApiResponse<PrimaryInvestorInfoResponse>.ErrorResponse(
                        "Investor profile not found. Please create investor profile first.",
                        404);
                }

                // Validate age (must be 18+)
                var age = DateTime.Today.Year - request.DateOfBirth.Year;
                if (request.DateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;

                if (age < 18)
                {
                    _logger.LogWarning("Investor age validation failed. Age: {Age}", age);
                    return ApiResponse<PrimaryInvestorInfoResponse>.ErrorResponse(
                        "Investor must be at least 18 years old.",
                        400);
                }

                // Validate Driver's License expiration date (must be in future)
                if (request.DriversLicenseExpirationDate.Date <= DateTime.Today)
                {
                    _logger.LogWarning("Driver's license expiration date validation failed. Date: {ExpirationDate}", request.DriversLicenseExpirationDate);
                    return ApiResponse<PrimaryInvestorInfoResponse>.ErrorResponse(
                        "Driver's license expiration date must be in the future.",
                        400);
                }

                // Check if Primary Investor Info already exists (one-to-one relationship)
                var existingInfo = await _context.PrimaryInvestorInfos
                    .Include(p => p.BrokerAffiliations)
                    .Include(p => p.InvestmentExperiences)
                    .Include(p => p.SourceOfFunds)
                    .Include(p => p.TaxRates)
                    .FirstOrDefaultAsync(p => p.InvestorProfileId == request.InvestorProfileId);

                if (existingInfo != null)
                {
                    // Update existing record
                    _mapper.Map(request, existingInfo);
                    existingInfo.UpdatedAt = DateTime.UtcNow;

                    _context.PrimaryInvestorInfos.Update(existingInfo);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Primary Investor Info updated successfully. ID: {Id}", existingInfo.Id);

                    var updatedResponse = _mapper.Map<PrimaryInvestorInfoResponse>(existingInfo);
                    return ApiResponse<PrimaryInvestorInfoResponse>.SuccessResponse(
                        updatedResponse,
                        "Primary investor information updated successfully.",
                        200);
                }
                else
                {
                    // Create new record
                    var newInfo = _mapper.Map<TWS.Data.Entities.PrimaryInvestorInfo.PrimaryInvestorInfo>(request);
                    newInfo.CreatedAt = DateTime.UtcNow;

                    await _context.PrimaryInvestorInfos.AddAsync(newInfo);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Primary Investor Info created successfully. ID: {Id}", newInfo.Id);

                    // Load with relationships for response
                    var savedInfo = await _context.PrimaryInvestorInfos
                        .Include(p => p.BrokerAffiliations)
                        .Include(p => p.InvestmentExperiences)
                        .Include(p => p.SourceOfFunds)
                        .Include(p => p.TaxRates)
                        .FirstOrDefaultAsync(p => p.Id == newInfo.Id);

                    var response = _mapper.Map<PrimaryInvestorInfoResponse>(savedInfo);
                    return ApiResponse<PrimaryInvestorInfoResponse>.SuccessResponse(
                        response,
                        "Primary investor information saved successfully.",
                        201);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Primary Investor Info for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);
                return ApiResponse<PrimaryInvestorInfoResponse>.ErrorResponse(
                    "An error occurred while saving primary investor information.",
                    500);
            }
        }

        /// <summary>
        /// Saves or updates the broker affiliation information
        /// </summary>
        public async Task<ApiResponse<BrokerAffiliationResponse>> SaveBrokerAffiliationAsync(SaveBrokerAffiliationRequest request)
        {
            try
            {
                _logger.LogInformation("Saving Broker Affiliation for PrimaryInvestorInfoId: {PrimaryInvestorInfoId}", request.PrimaryInvestorInfoId);

                // Validate Primary Investor Info exists
                var primaryInfo = await _context.PrimaryInvestorInfos
                    .Include(p => p.BrokerAffiliations)
                    .FirstOrDefaultAsync(p => p.Id == request.PrimaryInvestorInfoId);

                if (primaryInfo == null)
                {
                    _logger.LogWarning("Primary Investor Info not found with ID: {PrimaryInvestorInfoId}", request.PrimaryInvestorInfoId);
                    return ApiResponse<BrokerAffiliationResponse>.ErrorResponse(
                        "Primary investor information not found.",
                        404);
                }

                // Delete existing broker affiliation if exists (only one per investor)
                var existingAffiliation = primaryInfo.BrokerAffiliations.FirstOrDefault();
                if (existingAffiliation != null)
                {
                    _context.BrokerAffiliations.Remove(existingAffiliation);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Existing Broker Affiliation deleted. ID: {Id}", existingAffiliation.Id);
                }

                // Create new broker affiliation
                var newAffiliation = _mapper.Map<BrokerAffiliation>(request);
                newAffiliation.CreatedAt = DateTime.UtcNow;

                await _context.BrokerAffiliations.AddAsync(newAffiliation);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Broker Affiliation created successfully. ID: {Id}", newAffiliation.Id);

                var response = _mapper.Map<BrokerAffiliationResponse>(newAffiliation);
                return ApiResponse<BrokerAffiliationResponse>.SuccessResponse(
                    response,
                    "Broker affiliation information saved successfully.",
                    201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Broker Affiliation for PrimaryInvestorInfoId: {PrimaryInvestorInfoId}", request.PrimaryInvestorInfoId);
                return ApiResponse<BrokerAffiliationResponse>.ErrorResponse(
                    "An error occurred while saving broker affiliation information.",
                    500);
            }
        }

        /// <summary>
        /// Saves investment experience information (replaces existing)
        /// </summary>
        public async Task<ApiResponse<List<InvestmentExperienceResponse>>> SaveInvestmentExperienceAsync(SaveInvestmentExperienceRequest request)
        {
            try
            {
                _logger.LogInformation("Saving Investment Experiences for PrimaryInvestorInfoId: {PrimaryInvestorInfoId}", request.PrimaryInvestorInfoId);

                // Validate Primary Investor Info exists
                var primaryInfo = await _context.PrimaryInvestorInfos
                    .Include(p => p.InvestmentExperiences)
                    .FirstOrDefaultAsync(p => p.Id == request.PrimaryInvestorInfoId);

                if (primaryInfo == null)
                {
                    _logger.LogWarning("Primary Investor Info not found with ID: {PrimaryInvestorInfoId}", request.PrimaryInvestorInfoId);
                    return ApiResponse<List<InvestmentExperienceResponse>>.ErrorResponse(
                        "Primary investor information not found.",
                        404);
                }

                // Clear existing investment experiences
                if (primaryInfo.InvestmentExperiences.Any())
                {
                    _context.InvestmentExperiences.RemoveRange(primaryInfo.InvestmentExperiences);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Existing Investment Experiences cleared for PrimaryInvestorInfoId: {PrimaryInvestorInfoId}", request.PrimaryInvestorInfoId);
                }

                // Add new investment experiences
                var newExperiences = new List<InvestmentExperience>();
                foreach (var item in request.Experiences)
                {
                    var experience = _mapper.Map<InvestmentExperience>(item);
                    experience.PrimaryInvestorInfoId = request.PrimaryInvestorInfoId;
                    experience.CreatedAt = DateTime.UtcNow;
                    newExperiences.Add(experience);
                }

                await _context.InvestmentExperiences.AddRangeAsync(newExperiences);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Investment Experiences saved successfully. Count: {Count}", newExperiences.Count);

                var response = _mapper.Map<List<InvestmentExperienceResponse>>(newExperiences);
                return ApiResponse<List<InvestmentExperienceResponse>>.SuccessResponse(
                    response,
                    "Investment experience information saved successfully.",
                    201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Investment Experiences for PrimaryInvestorInfoId: {PrimaryInvestorInfoId}", request.PrimaryInvestorInfoId);
                return ApiResponse<List<InvestmentExperienceResponse>>.ErrorResponse(
                    "An error occurred while saving investment experience information.",
                    500);
            }
        }

        /// <summary>
        /// Saves source of funds information (replaces existing)
        /// </summary>
        public async Task<ApiResponse<List<SourceOfFundsResponse>>> SaveSourceOfFundsAsync(SaveSourceOfFundsRequest request)
        {
            try
            {
                _logger.LogInformation("Saving Source of Funds for PrimaryInvestorInfoId: {PrimaryInvestorInfoId}", request.PrimaryInvestorInfoId);

                // Validate Primary Investor Info exists
                var primaryInfo = await _context.PrimaryInvestorInfos
                    .Include(p => p.SourceOfFunds)
                    .FirstOrDefaultAsync(p => p.Id == request.PrimaryInvestorInfoId);

                if (primaryInfo == null)
                {
                    _logger.LogWarning("Primary Investor Info not found with ID: {PrimaryInvestorInfoId}", request.PrimaryInvestorInfoId);
                    return ApiResponse<List<SourceOfFundsResponse>>.ErrorResponse(
                        "Primary investor information not found.",
                        404);
                }

                // Clear existing source of funds
                if (primaryInfo.SourceOfFunds.Any())
                {
                    _context.SourceOfFunds.RemoveRange(primaryInfo.SourceOfFunds);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Existing Source of Funds cleared for PrimaryInvestorInfoId: {PrimaryInvestorInfoId}", request.PrimaryInvestorInfoId);
                }

                // Add new source of funds
                var newSourceOfFunds = new List<SourceOfFunds>();
                foreach (var sourceType in request.SourceTypes)
                {
                    var source = new SourceOfFunds
                    {
                        PrimaryInvestorInfoId = request.PrimaryInvestorInfoId,
                        SourceType = (SourceOfFundsType)sourceType,
                        CreatedAt = DateTime.UtcNow
                    };
                    newSourceOfFunds.Add(source);
                }

                await _context.SourceOfFunds.AddRangeAsync(newSourceOfFunds);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Source of Funds saved successfully. Count: {Count}", newSourceOfFunds.Count);

                var response = _mapper.Map<List<SourceOfFundsResponse>>(newSourceOfFunds);
                return ApiResponse<List<SourceOfFundsResponse>>.SuccessResponse(
                    response,
                    "Source of funds information saved successfully.",
                    201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Source of Funds for PrimaryInvestorInfoId: {PrimaryInvestorInfoId}", request.PrimaryInvestorInfoId);
                return ApiResponse<List<SourceOfFundsResponse>>.ErrorResponse(
                    "An error occurred while saving source of funds information.",
                    500);
            }
        }

        /// <summary>
        /// Saves tax rates information (replaces existing)
        /// </summary>
        public async Task<ApiResponse<List<TaxRateResponse>>> SaveTaxRatesAsync(SaveTaxRatesRequest request)
        {
            try
            {
                _logger.LogInformation("Saving Tax Rates for PrimaryInvestorInfoId: {PrimaryInvestorInfoId}", request.PrimaryInvestorInfoId);

                // Validate Primary Investor Info exists
                var primaryInfo = await _context.PrimaryInvestorInfos
                    .Include(p => p.TaxRates)
                    .FirstOrDefaultAsync(p => p.Id == request.PrimaryInvestorInfoId);

                if (primaryInfo == null)
                {
                    _logger.LogWarning("Primary Investor Info not found with ID: {PrimaryInvestorInfoId}", request.PrimaryInvestorInfoId);
                    return ApiResponse<List<TaxRateResponse>>.ErrorResponse(
                        "Primary investor information not found.",
                        404);
                }

                // Clear existing tax rates
                if (primaryInfo.TaxRates.Any())
                {
                    _context.TaxRates.RemoveRange(primaryInfo.TaxRates);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Existing Tax Rates cleared for PrimaryInvestorInfoId: {PrimaryInvestorInfoId}", request.PrimaryInvestorInfoId);
                }

                // Add new tax rates
                var newTaxRates = new List<TaxRate>();
                foreach (var taxRateRange in request.TaxRateRanges)
                {
                    var taxRate = new TaxRate
                    {
                        PrimaryInvestorInfoId = request.PrimaryInvestorInfoId,
                        TaxRateRange = (TaxRateRange)taxRateRange,
                        CreatedAt = DateTime.UtcNow
                    };
                    newTaxRates.Add(taxRate);
                }

                await _context.TaxRates.AddRangeAsync(newTaxRates);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Tax Rates saved successfully. Count: {Count}", newTaxRates.Count);

                var response = _mapper.Map<List<TaxRateResponse>>(newTaxRates);
                return ApiResponse<List<TaxRateResponse>>.SuccessResponse(
                    response,
                    "Tax rates information saved successfully.",
                    201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Tax Rates for PrimaryInvestorInfoId: {PrimaryInvestorInfoId}", request.PrimaryInvestorInfoId);
                return ApiResponse<List<TaxRateResponse>>.ErrorResponse(
                    "An error occurred while saving tax rates information.",
                    500);
            }
        }

        /// <summary>
        /// Gets primary investor info by investor profile ID
        /// </summary>
        public async Task<ApiResponse<PrimaryInvestorInfoResponse>> GetByInvestorProfileIdAsync(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Getting Primary Investor Info for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                var primaryInfo = await _context.PrimaryInvestorInfos
                    .Include(p => p.BrokerAffiliations)
                    .Include(p => p.InvestmentExperiences)
                    .Include(p => p.SourceOfFunds)
                    .Include(p => p.TaxRates)
                    .FirstOrDefaultAsync(p => p.InvestorProfileId == investorProfileId);

                if (primaryInfo == null)
                {
                    _logger.LogWarning("Primary Investor Info not found for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                    return ApiResponse<PrimaryInvestorInfoResponse>.ErrorResponse(
                        "Primary investor information not found.",
                        404);
                }

                var response = _mapper.Map<PrimaryInvestorInfoResponse>(primaryInfo);
                return ApiResponse<PrimaryInvestorInfoResponse>.SuccessResponse(
                    response,
                    "Primary investor information retrieved successfully.",
                    200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Primary Investor Info for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return ApiResponse<PrimaryInvestorInfoResponse>.ErrorResponse(
                    "An error occurred while retrieving primary investor information.",
                    500);
            }
        }
    }
}