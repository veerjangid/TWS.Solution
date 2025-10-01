using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWS.Core.DTOs.Request.FinancialGoals;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.FinancialGoals;
using TWS.Core.Interfaces.IServices;

namespace TWS.API.Controllers
{
    /// <summary>
    /// Controller for Financial Goals operations.
    /// Handles creation, retrieval, update, and deletion of investor financial goals.
    /// Reference: APIDoc.md Section 9, BusinessRequirement.md Section 10
    /// </summary>
    [ApiController]
    [Route("api/financial-goals")]
    public class FinancialGoalsController : ControllerBase
    {
        private readonly IFinancialGoalsService _financialGoalsService;
        private readonly ILogger<FinancialGoalsController> _logger;

        public FinancialGoalsController(
            IFinancialGoalsService financialGoalsService,
            ILogger<FinancialGoalsController> logger)
        {
            _financialGoalsService = financialGoalsService ?? throw new ArgumentNullException(nameof(financialGoalsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Save financial goals for an investor profile.
        /// Creates new record if none exists, updates existing record if one exists (one-to-one).
        /// </summary>
        /// <param name="request">Financial goals data</param>
        /// <returns>Saved financial goals details</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<FinancialGoalsResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<FinancialGoalsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SaveFinancialGoals([FromBody] SaveFinancialGoalsRequest request)
        {
            try
            {
                _logger.LogInformation("Saving financial goals for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);

                // Model validation is handled by DataAnnotations
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for SaveFinancialGoals request");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Data = ModelState
                    });
                }

                var response = await _financialGoalsService.SaveFinancialGoalsAsync(request);

                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving financial goals for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while saving financial goals",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Retrieve financial goals for a specific investor profile.
        /// </summary>
        /// <param name="investorProfileId">The ID of the investor profile</param>
        /// <returns>Financial goals details</returns>
        [HttpGet("investor/{investorProfileId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<FinancialGoalsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetByInvestorProfileId(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Retrieving financial goals for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                if (investorProfileId <= 0)
                {
                    _logger.LogWarning("Invalid InvestorProfileId: {InvestorProfileId}", investorProfileId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "InvestorProfileId must be a positive integer",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                var response = await _financialGoalsService.GetByInvestorProfileIdAsync(investorProfileId);

                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving financial goals for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while retrieving financial goals",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Delete financial goals for a specific investor profile.
        /// </summary>
        /// <param name="investorProfileId">The ID of the investor profile</param>
        /// <returns>Success status</returns>
        [HttpDelete("{investorProfileId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteFinancialGoals(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Deleting financial goals for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                if (investorProfileId <= 0)
                {
                    _logger.LogWarning("Invalid InvestorProfileId: {InvestorProfileId}", investorProfileId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "InvestorProfileId must be a positive integer",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                var response = await _financialGoalsService.DeleteFinancialGoalsAsync(investorProfileId);

                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting financial goals for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while deleting financial goals",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}