using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Net;
using TWS.Core.DTOs.Request.FinancialGoals;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.FinancialGoals;
using TWS.Core.Enums;
using TWS.Core.Interfaces.IRepositories;
using TWS.Core.Interfaces.IServices;
using TWS.Data.Entities.Financial;

namespace TWS.Infra.Services.Financial
{
    /// <summary>
    /// Service implementation for Financial Goals operations.
    /// Handles business logic for creating, retrieving, updating, and deleting financial goals.
    /// Reference: APIDoc.md Section 9, BusinessRequirement.md Section 10
    /// </summary>
    public class FinancialGoalsService : IFinancialGoalsService
    {
        private readonly IFinancialGoalsRepository _financialGoalsRepository;
        private readonly IInvestorProfileRepository _investorProfileRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FinancialGoalsService> _logger;

        public FinancialGoalsService(
            IFinancialGoalsRepository financialGoalsRepository,
            IInvestorProfileRepository investorProfileRepository,
            IMapper mapper,
            ILogger<FinancialGoalsService> logger)
        {
            _financialGoalsRepository = financialGoalsRepository ?? throw new ArgumentNullException(nameof(financialGoalsRepository));
            _investorProfileRepository = investorProfileRepository ?? throw new ArgumentNullException(nameof(investorProfileRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Saves financial goals for an investor profile.
        /// Creates new record if none exists, updates existing record if one exists (one-to-one relationship).
        /// </summary>
        public async Task<ApiResponse<FinancialGoalsResponse>> SaveFinancialGoalsAsync(SaveFinancialGoalsRequest request)
        {
            try
            {
                _logger.LogInformation("Saving financial goals for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);

                // Validate that the investor profile exists
                var investorProfile = await _investorProfileRepository.GetByIdAsync(request.InvestorProfileId);
                if (investorProfile == null)
                {
                    _logger.LogWarning("InvestorProfile not found for Id: {InvestorProfileId}", request.InvestorProfileId);
                    return ApiResponse<FinancialGoalsResponse>.ErrorResponse(
                        "Investor profile not found",
                        (int)HttpStatusCode.NotFound
                    );
                }

                // Check if financial goals already exist for this investor profile
                var existingGoals = await _financialGoalsRepository.GetByInvestorProfileIdAsync(request.InvestorProfileId);

                FinancialGoals goalsEntity;
                bool isUpdate = false;

                if (existingGoals != null)
                {
                    // Update existing financial goals
                    _logger.LogInformation("Updating existing financial goals for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);

                    var existingGoalsTyped = existingGoals as FinancialGoals;
                    if (existingGoalsTyped == null)
                    {
                        _logger.LogError("Failed to cast existing financial goals to FinancialGoals");
                        return ApiResponse<FinancialGoalsResponse>.ErrorResponse(
                            "Internal error processing existing financial goals",
                            (int)HttpStatusCode.InternalServerError
                        );
                    }

                    // Update properties
                    existingGoalsTyped.LiquidityNeeds = (LiquidityNeeds)request.LiquidityNeeds;
                    existingGoalsTyped.InvestmentTimeline = (InvestmentTimeline)request.InvestmentTimeline;
                    existingGoalsTyped.InvestmentObjective = (InvestmentObjective)request.InvestmentObjective;
                    existingGoalsTyped.RiskTolerance = (RiskTolerance)request.RiskTolerance;
                    existingGoalsTyped.DeferTaxes = request.DeferTaxes;
                    existingGoalsTyped.ProtectPrincipal = request.ProtectPrincipal;
                    existingGoalsTyped.GrowPrincipal = request.GrowPrincipal;
                    existingGoalsTyped.ConsistentCashFlow = request.ConsistentCashFlow;
                    existingGoalsTyped.Diversification = request.Diversification;
                    existingGoalsTyped.Retirement = request.Retirement;
                    existingGoalsTyped.EstateLegacyPlanning = request.EstateLegacyPlanning;
                    existingGoalsTyped.AdditionalNotes = request.AdditionalNotes;
                    existingGoalsTyped.UpdatedAt = DateTime.UtcNow;

                    await _financialGoalsRepository.UpdateAsync(existingGoalsTyped);
                    goalsEntity = existingGoalsTyped;
                    isUpdate = true;
                }
                else
                {
                    // Create new financial goals record
                    _logger.LogInformation("Creating new financial goals for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);

                    goalsEntity = new FinancialGoals
                    {
                        InvestorProfileId = request.InvestorProfileId,
                        LiquidityNeeds = (LiquidityNeeds)request.LiquidityNeeds,
                        InvestmentTimeline = (InvestmentTimeline)request.InvestmentTimeline,
                        InvestmentObjective = (InvestmentObjective)request.InvestmentObjective,
                        RiskTolerance = (RiskTolerance)request.RiskTolerance,
                        DeferTaxes = request.DeferTaxes,
                        ProtectPrincipal = request.ProtectPrincipal,
                        GrowPrincipal = request.GrowPrincipal,
                        ConsistentCashFlow = request.ConsistentCashFlow,
                        Diversification = request.Diversification,
                        Retirement = request.Retirement,
                        EstateLegacyPlanning = request.EstateLegacyPlanning,
                        AdditionalNotes = request.AdditionalNotes,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _financialGoalsRepository.AddAsync(goalsEntity);
                }

                // Save changes
                var saved = await _financialGoalsRepository.SaveChangesAsync();
                if (!saved)
                {
                    _logger.LogError("Failed to save financial goals for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);
                    return ApiResponse<FinancialGoalsResponse>.ErrorResponse(
                        "Failed to save financial goals",
                        (int)HttpStatusCode.InternalServerError
                    );
                }

                var response = _mapper.Map<FinancialGoalsResponse>(goalsEntity);

                _logger.LogInformation("Successfully saved financial goals for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);

                return ApiResponse<FinancialGoalsResponse>.SuccessResponse(
                    response,
                    isUpdate ? "Financial goals updated successfully" : "Financial goals created successfully",
                    isUpdate ? (int)HttpStatusCode.OK : (int)HttpStatusCode.Created
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving financial goals for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);
                return ApiResponse<FinancialGoalsResponse>.ErrorResponse(
                    "An error occurred while saving financial goals",
                    (int)HttpStatusCode.InternalServerError
                );
            }
        }

        /// <summary>
        /// Retrieves the financial goals for a specific investor profile.
        /// </summary>
        public async Task<ApiResponse<FinancialGoalsResponse>> GetByInvestorProfileIdAsync(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Retrieving financial goals for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                var goalsEntity = await _financialGoalsRepository.GetByInvestorProfileIdAsync(investorProfileId);

                if (goalsEntity == null)
                {
                    _logger.LogInformation("No financial goals found for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                    return ApiResponse<FinancialGoalsResponse>.ErrorResponse(
                        "Financial goals not found",
                        (int)HttpStatusCode.NotFound
                    );
                }

                var goalsTyped = goalsEntity as FinancialGoals;
                if (goalsTyped == null)
                {
                    _logger.LogError("Failed to cast financial goals to FinancialGoals");
                    return ApiResponse<FinancialGoalsResponse>.ErrorResponse(
                        "Internal error processing financial goals",
                        (int)HttpStatusCode.InternalServerError
                    );
                }

                var response = _mapper.Map<FinancialGoalsResponse>(goalsTyped);

                _logger.LogInformation("Successfully retrieved financial goals for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                return ApiResponse<FinancialGoalsResponse>.SuccessResponse(
                    response,
                    "Financial goals retrieved successfully",
                    (int)HttpStatusCode.OK
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving financial goals for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return ApiResponse<FinancialGoalsResponse>.ErrorResponse(
                    "An error occurred while retrieving financial goals",
                    (int)HttpStatusCode.InternalServerError
                );
            }
        }

        /// <summary>
        /// Deletes the financial goals for a specific investor profile.
        /// </summary>
        public async Task<ApiResponse<bool>> DeleteFinancialGoalsAsync(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Deleting financial goals for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                var goalsEntity = await _financialGoalsRepository.GetByInvestorProfileIdAsync(investorProfileId);

                if (goalsEntity == null)
                {
                    _logger.LogWarning("No financial goals found to delete for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                    return ApiResponse<bool>.ErrorResponse(
                        "Financial goals not found",
                        (int)HttpStatusCode.NotFound
                    );
                }

                var goalsTyped = goalsEntity as FinancialGoals;
                if (goalsTyped == null)
                {
                    _logger.LogError("Failed to cast financial goals to FinancialGoals");
                    return ApiResponse<bool>.ErrorResponse(
                        "Internal error processing financial goals",
                        (int)HttpStatusCode.InternalServerError
                    );
                }

                // Delete database record
                await _financialGoalsRepository.DeleteAsync(goalsTyped);
                var saved = await _financialGoalsRepository.SaveChangesAsync();

                if (!saved)
                {
                    _logger.LogError("Failed to delete financial goals for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                    return ApiResponse<bool>.ErrorResponse(
                        "Failed to delete financial goals",
                        (int)HttpStatusCode.InternalServerError
                    );
                }

                _logger.LogInformation("Successfully deleted financial goals for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                return ApiResponse<bool>.SuccessResponse(
                    true,
                    "Financial goals deleted successfully",
                    (int)HttpStatusCode.OK
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting financial goals for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return ApiResponse<bool>.ErrorResponse(
                    "An error occurred while deleting financial goals",
                    (int)HttpStatusCode.InternalServerError
                );
            }
        }
    }
}