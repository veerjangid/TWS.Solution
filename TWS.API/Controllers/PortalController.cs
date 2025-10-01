using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWS.Core.Constants;
using TWS.Core.DTOs.Request.Portal;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Portal;
using TWS.Core.Interfaces.IServices;

namespace TWS.API.Controllers
{
    /// <summary>
    /// Manages Portal/CRM investment tracking functionality.
    /// CRITICAL: This controller is ONLY accessible to Advisor and OperationsTeam roles.
    /// Investors do NOT have access to this Portal CRM module.
    /// Reference: APIDoc.md Section 13, BusinessRequirement.md Section 14
    /// </summary>
    [ApiController]
    [Route("api/portal")]
    [Authorize(Roles = $"{RoleConstants.Advisor},{RoleConstants.OperationsTeam}")]
    public class PortalController : ControllerBase
    {
        private readonly IPortalService _portalService;
        private readonly ILogger<PortalController> _logger;

        public PortalController(
            IPortalService portalService,
            ILogger<PortalController> logger)
        {
            _portalService = portalService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new investment tracker for Portal/CRM.
        /// Tracks investment with complex financial metrics including AUM, revenue, and commissions.
        /// Only accessible to Advisor and OperationsTeam roles.
        /// </summary>
        /// <param name="request">Investment tracker creation request with 26+ fields</param>
        /// <returns>Created tracker with ID</returns>
        /// <response code="201">Investment tracker created successfully</response>
        /// <response code="400">Invalid request data or validation errors</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="403">Forbidden - requires Advisor or OperationsTeam role</response>
        /// <response code="404">Offering or Investor Profile not found</response>
        [HttpPost("investment-tracker")]
        [ProducesResponseType(typeof(ApiResponse<InvestmentTrackerResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<InvestmentTrackerResponse>>> CreateInvestmentTracker(
            [FromBody] CreateInvestmentTrackerRequest request)
        {
            try
            {
                _logger.LogInformation("Creating investment tracker for Offering {OfferingId} and InvestorProfile {InvestorProfileId}",
                    request.OfferingId, request.InvestorProfileId);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _portalService.CreateTrackerAsync(request);

                if (!result.Success)
                {
                    return result.StatusCode switch
                    {
                        404 => NotFound(result),
                        _ => StatusCode(result.StatusCode, result)
                    };
                }

                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating investment tracker");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<InvestmentTrackerResponse>.ErrorResponse(
                        "An error occurred while creating the investment tracker",
                        500
                    ));
            }
        }

        /// <summary>
        /// Updates the status of an investment tracker.
        /// Allows status changes with optional notes for tracking investment progress.
        /// Only accessible to Advisor and OperationsTeam roles.
        /// </summary>
        /// <param name="id">Investment tracker ID</param>
        /// <param name="request">Status update request with new status and optional notes</param>
        /// <returns>Updated tracker details</returns>
        /// <response code="200">Status updated successfully</response>
        /// <response code="400">Invalid request data or validation errors</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="403">Forbidden - requires Advisor or OperationsTeam role</response>
        /// <response code="404">Investment tracker not found</response>
        [HttpPatch("investment-tracker/{id}/status")]
        [ProducesResponseType(typeof(ApiResponse<InvestmentTrackerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<InvestmentTrackerResponse>>> UpdateTrackerStatus(
            int id,
            [FromBody] UpdateTrackerStatusRequest request)
        {
            try
            {
                _logger.LogInformation("Updating status for tracker {TrackerId}", id);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _portalService.UpdateStatusAsync(id, request);

                if (!result.Success)
                {
                    return result.StatusCode switch
                    {
                        404 => NotFound(result),
                        _ => StatusCode(result.StatusCode, result)
                    };
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tracker {TrackerId} status", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<InvestmentTrackerResponse>.ErrorResponse(
                        "An error occurred while updating the investment tracker status",
                        500
                    ));
            }
        }

        /// <summary>
        /// Gets all investment trackers for Portal/CRM dashboard view.
        /// Returns summary view with essential fields for dashboard display.
        /// Only accessible to Advisor and OperationsTeam roles.
        /// </summary>
        /// <returns>Collection of dashboard items with summary information</returns>
        /// <response code="200">Dashboard items retrieved successfully</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="403">Forbidden - requires Advisor or OperationsTeam role</response>
        [HttpGet("dashboard")]
        [ProducesResponseType(typeof(ApiResponse<List<DashboardItemResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<List<DashboardItemResponse>>>> GetDashboard()
        {
            try
            {
                _logger.LogInformation("Retrieving dashboard items");

                var result = await _portalService.GetDashboardAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard items");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<List<DashboardItemResponse>>.ErrorResponse(
                        "An error occurred while retrieving dashboard items",
                        500
                    ));
            }
        }

        /// <summary>
        /// Gets an investment tracker by ID with full details.
        /// Returns all 26+ fields including financial metrics, AUM, and revenue data.
        /// Only accessible to Advisor and OperationsTeam roles.
        /// </summary>
        /// <param name="id">Investment tracker ID</param>
        /// <returns>Complete tracker details</returns>
        /// <response code="200">Tracker details retrieved successfully</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="403">Forbidden - requires Advisor or OperationsTeam role</response>
        /// <response code="404">Investment tracker not found</response>
        [HttpGet("investment-tracker/{id}")]
        [ProducesResponseType(typeof(ApiResponse<InvestmentTrackerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<InvestmentTrackerResponse>>> GetTrackerById(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving tracker {TrackerId}", id);

                var result = await _portalService.GetTrackerByIdAsync(id);

                if (!result.Success)
                {
                    return result.StatusCode switch
                    {
                        404 => NotFound(result),
                        _ => StatusCode(result.StatusCode, result)
                    };
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tracker {TrackerId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<InvestmentTrackerResponse>.ErrorResponse(
                        "An error occurred while retrieving the investment tracker",
                        500
                    ));
            }
        }

        /// <summary>
        /// Deletes an investment tracker.
        /// Permanently removes the tracker from the Portal/CRM system.
        /// Only accessible to Advisor and OperationsTeam roles.
        /// </summary>
        /// <param name="id">Investment tracker ID</param>
        /// <returns>Deletion success status</returns>
        /// <response code="200">Tracker deleted successfully</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="403">Forbidden - requires Advisor or OperationsTeam role</response>
        /// <response code="404">Investment tracker not found</response>
        [HttpDelete("investment-tracker/{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteTracker(int id)
        {
            try
            {
                _logger.LogInformation("Deleting tracker {TrackerId}", id);

                var result = await _portalService.DeleteTrackerAsync(id);

                if (!result.Success)
                {
                    return result.StatusCode switch
                    {
                        404 => NotFound(result),
                        _ => StatusCode(result.StatusCode, result)
                    };
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting tracker {TrackerId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<bool>.ErrorResponse(
                        "An error occurred while deleting the investment tracker",
                        500
                    ));
            }
        }
    }
}