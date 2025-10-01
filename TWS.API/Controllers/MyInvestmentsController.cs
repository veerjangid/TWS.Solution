using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWS.Core.DTOs.Request.Investment;
using TWS.Core.DTOs.Response.Investment;
using TWS.Core.Interfaces.IServices;

namespace TWS.API.Controllers
{
    /// <summary>
    /// Manages investor investments in offerings.
    /// Handles creating investments and tracking investment status.
    /// Reference: APIDoc.md Section 12
    /// </summary>
    [ApiController]
    [Route("api/my-investments")]
    [Authorize]
    public class MyInvestmentsController : ControllerBase
    {
        private readonly IInvestmentService _investmentService;
        private readonly ILogger<MyInvestmentsController> _logger;

        public MyInvestmentsController(
            IInvestmentService investmentService,
            ILogger<MyInvestmentsController> logger)
        {
            _investmentService = investmentService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new investment for an investor in an offering.
        /// Validates that investor hasn't already invested in the same offering.
        /// </summary>
        /// <param name="request">Investment details including investor profile ID, offering ID, and amount</param>
        /// <returns>Created investment details</returns>
        /// <response code="201">Investment successfully created</response>
        /// <response code="400">Validation failed or duplicate investment</response>
        /// <response code="401">Unauthorized - authentication required</response>
        [HttpPost]
        [ProducesResponseType(typeof(InvestmentResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<InvestmentResponse>> CreateInvestment(
            [FromBody] CreateInvestmentRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    _logger.LogWarning("Invalid model state for creating investment: {Errors}", string.Join(", ", errors));

                    return BadRequest(new
                    {
                        success = false,
                        message = "Validation failed",
                        errors = errors
                    });
                }

                _logger.LogInformation(
                    "Creating investment for InvestorProfileId: {InvestorProfileId}, OfferingId: {OfferingId}",
                    request.InvestorProfileId,
                    request.OfferingId);

                var investment = await _investmentService.CreateInvestmentAsync(request);

                _logger.LogInformation("Investment {InvestmentId} created successfully", investment.Id);

                return CreatedAtAction(
                    nameof(GetInvestmentById),
                    new { id = investment.Id },
                    investment);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while creating investment");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating investment");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while creating the investment" });
            }
        }

        /// <summary>
        /// Gets all investments for a specific investor profile.
        /// Returns investments sorted by investment date (newest first).
        /// </summary>
        /// <param name="investorProfileId">Investor profile ID</param>
        /// <returns>Collection of investments for the investor</returns>
        /// <response code="200">Successfully retrieved investments</response>
        /// <response code="401">Unauthorized - authentication required</response>
        [HttpGet("investor/{investorProfileId}")]
        [ProducesResponseType(typeof(IEnumerable<InvestmentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<InvestmentResponse>>> GetInvestmentsByInvestorProfile(
            int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Retrieving investments for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                var investments = await _investmentService.GetByInvestorProfileIdAsync(investorProfileId);

                return Ok(investments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving investments for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving investments" });
            }
        }

        /// <summary>
        /// Gets detailed information about a specific investment.
        /// Includes offering details and investor information.
        /// </summary>
        /// <param name="id">Investment ID</param>
        /// <returns>Investment details</returns>
        /// <response code="200">Successfully retrieved investment</response>
        /// <response code="404">Investment not found</response>
        /// <response code="401">Unauthorized - authentication required</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InvestmentDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<InvestmentDetailsResponse>> GetInvestmentById(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving investment details for InvestmentId: {InvestmentId}", id);

                var investment = await _investmentService.GetInvestmentDetailsAsync(id);

                if (investment == null)
                {
                    _logger.LogWarning("Investment {InvestmentId} not found", id);
                    return NotFound(new { message = $"Investment with ID {id} not found" });
                }

                return Ok(investment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving investment details for InvestmentId: {InvestmentId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving investment details" });
            }
        }

        /// <summary>
        /// Updates the status of an investment.
        /// Used by operations team to track investment progress through various stages.
        /// </summary>
        /// <param name="id">Investment ID</param>
        /// <param name="request">Status update request</param>
        /// <returns>Updated investment details</returns>
        /// <response code="200">Successfully updated investment status</response>
        /// <response code="400">Invalid status value</response>
        /// <response code="404">Investment not found</response>
        /// <response code="401">Unauthorized - authentication required</response>
        [HttpPatch("{id}/status")]
        [ProducesResponseType(typeof(InvestmentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<InvestmentResponse>> UpdateInvestmentStatus(
            int id,
            [FromBody] UpdateInvestmentStatusRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    _logger.LogWarning("Invalid model state for updating investment status: {Errors}", string.Join(", ", errors));

                    return BadRequest(new
                    {
                        success = false,
                        message = "Validation failed",
                        errors = errors
                    });
                }

                _logger.LogInformation("Updating status for InvestmentId: {InvestmentId} to {Status}", id, request.Status);

                var investment = await _investmentService.UpdateStatusAsync(id, request);

                _logger.LogInformation("Investment {InvestmentId} status updated successfully", id);

                return Ok(investment);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid status value for investment update");
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Investment not found for status update");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating investment status for InvestmentId: {InvestmentId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while updating investment status" });
            }
        }
    }
}