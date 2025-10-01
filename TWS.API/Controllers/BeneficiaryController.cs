using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWS.Core.DTOs.Request.Beneficiary;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Beneficiary;
using TWS.Core.Interfaces.IServices;

namespace TWS.API.Controllers
{
    /// <summary>
    /// Manages beneficiary information for investor profiles.
    /// Handles CRUD operations and percentage validation for Primary and Contingent beneficiaries.
    /// </summary>
    [ApiController]
    [Route("api/beneficiary")]
    [Authorize]
    public class BeneficiaryController : ControllerBase
    {
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly ILogger<BeneficiaryController> _logger;

        public BeneficiaryController(
            IBeneficiaryService beneficiaryService,
            ILogger<BeneficiaryController> logger)
        {
            _beneficiaryService = beneficiaryService;
            _logger = logger;
        }

        /// <summary>
        /// Adds a single beneficiary to an investor profile.
        /// Validates that adding this beneficiary does not cause total percentage to exceed 100% for the beneficiary type.
        /// </summary>
        /// <param name="request">Beneficiary details including name, relationship, percentage, and type</param>
        /// <returns>Created beneficiary with assigned ID</returns>
        /// <response code="201">Beneficiary successfully created</response>
        /// <response code="400">Validation failed or percentage would exceed 100%</response>
        /// <response code="401">Unauthorized - authentication required</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<BeneficiaryResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<BeneficiaryResponse>>> AddBeneficiary(
            [FromBody] AddBeneficiaryRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    _logger.LogWarning("Invalid model state for adding beneficiary: {Errors}", string.Join(", ", errors));

                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = errors,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                _logger.LogInformation(
                    "Adding beneficiary for InvestorProfileId: {InvestorProfileId}, Type: {BeneficiaryType}",
                    request.InvestorProfileId,
                    request.BeneficiaryType);

                var response = await _beneficiaryService.AddBeneficiaryAsync(request);

                if (!response.Success)
                {
                    _logger.LogWarning(
                        "Failed to add beneficiary: {Message}",
                        response.Message);

                    return BadRequest(response);
                }

                _logger.LogInformation(
                    "Successfully added beneficiary with ID: {BeneficiaryId}",
                    response.Data?.Id);

                return Created($"/api/beneficiary/{response.Data?.Id}", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding beneficiary for InvestorProfileId: {InvestorProfileId}",
                    request.InvestorProfileId);

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while adding the beneficiary",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Adds multiple beneficiaries to an investor profile in a single operation.
        /// Validates that total percentages equal exactly 100% for each beneficiary type (Primary and Contingent separately).
        /// REPLACES all existing beneficiaries of the provided types.
        /// </summary>
        /// <param name="request">List of beneficiaries with percentage allocations</param>
        /// <returns>List of created beneficiaries with assigned IDs</returns>
        /// <response code="201">Beneficiaries successfully created</response>
        /// <response code="400">Validation failed or percentages do not equal 100% per type</response>
        /// <response code="401">Unauthorized - authentication required</response>
        [HttpPost("multiple")]
        [ProducesResponseType(typeof(ApiResponse<List<BeneficiaryResponse>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<List<BeneficiaryResponse>>>> AddMultipleBeneficiaries(
            [FromBody] AddMultipleBeneficiariesRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    _logger.LogWarning("Invalid model state for adding multiple beneficiaries: {Errors}",
                        string.Join(", ", errors));

                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = errors,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                if (request.Beneficiaries == null || !request.Beneficiaries.Any())
                {
                    _logger.LogWarning("No beneficiaries provided in request");

                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "At least one beneficiary is required",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                _logger.LogInformation(
                    "Adding {Count} beneficiaries for InvestorProfileId: {InvestorProfileId}",
                    request.Beneficiaries.Count,
                    request.InvestorProfileId);

                var response = await _beneficiaryService.AddMultipleBeneficiariesAsync(request);

                if (!response.Success)
                {
                    _logger.LogWarning(
                        "Failed to add multiple beneficiaries: {Message}",
                        response.Message);

                    return BadRequest(response);
                }

                _logger.LogInformation(
                    "Successfully added {Count} beneficiaries",
                    response.Data?.Count ?? 0);

                return Created($"/api/beneficiary/investor/{request.InvestorProfileId}", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding multiple beneficiaries for InvestorProfileId: {InvestorProfileId}",
                    request.InvestorProfileId);

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while adding beneficiaries",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Updates an existing beneficiary's information.
        /// Validates that the updated percentage does not cause total to exceed 100% for the beneficiary type.
        /// </summary>
        /// <param name="id">Beneficiary ID to update</param>
        /// <param name="request">Updated beneficiary details</param>
        /// <returns>Updated beneficiary information</returns>
        /// <response code="200">Beneficiary successfully updated</response>
        /// <response code="400">Validation failed or percentage would exceed 100%</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="404">Beneficiary not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<BeneficiaryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<BeneficiaryResponse>>> UpdateBeneficiary(
            int id,
            [FromBody] UpdateBeneficiaryRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    _logger.LogWarning("Invalid model state for updating beneficiary {BeneficiaryId}: {Errors}",
                        id, string.Join(", ", errors));

                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = errors,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                _logger.LogInformation("Updating beneficiary with ID: {BeneficiaryId}", id);

                var response = await _beneficiaryService.UpdateBeneficiaryAsync(id, request);

                if (!response.Success)
                {
                    _logger.LogWarning(
                        "Failed to update beneficiary {BeneficiaryId}: {Message}",
                        id,
                        response.Message);

                    // Check if it's a not found error
                    if (response.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    {
                        return NotFound(new ApiResponse<object>
                        {
                            Success = false,
                            Message = response.Message,
                            StatusCode = StatusCodes.Status404NotFound
                        });
                    }

                    return BadRequest(response);
                }

                _logger.LogInformation("Successfully updated beneficiary with ID: {BeneficiaryId}", id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating beneficiary with ID: {BeneficiaryId}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while updating the beneficiary",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Deletes a beneficiary from an investor profile.
        /// WARNING: Deleting a beneficiary may cause remaining percentages to not equal 100% for that type.
        /// Consider using the multiple beneficiaries endpoint to maintain proper percentage allocation.
        /// </summary>
        /// <param name="id">Beneficiary ID to delete</param>
        /// <returns>Success status</returns>
        /// <response code="200">Beneficiary successfully deleted</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="404">Beneficiary not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteBeneficiary(int id)
        {
            try
            {
                _logger.LogInformation("Deleting beneficiary with ID: {BeneficiaryId}", id);

                var response = await _beneficiaryService.DeleteBeneficiaryAsync(id);

                if (!response.Success)
                {
                    _logger.LogWarning(
                        "Failed to delete beneficiary {BeneficiaryId}: {Message}",
                        id,
                        response.Message);

                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = response.Message,
                        StatusCode = StatusCodes.Status404NotFound
                    });
                }

                _logger.LogInformation("Successfully deleted beneficiary with ID: {BeneficiaryId}", id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting beneficiary with ID: {BeneficiaryId}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while deleting the beneficiary",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Retrieves all beneficiaries for an investor profile.
        /// Results are ordered by BeneficiaryType, then by PercentageOfBenefit descending.
        /// </summary>
        /// <param name="investorProfileId">Investor profile ID</param>
        /// <returns>List of beneficiaries (can be empty)</returns>
        /// <response code="200">Beneficiaries retrieved successfully</response>
        /// <response code="401">Unauthorized - authentication required</response>
        [HttpGet("investor/{investorProfileId}")]
        [ProducesResponseType(typeof(ApiResponse<List<BeneficiaryResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<List<BeneficiaryResponse>>>> GetBeneficiariesByInvestor(
            int investorProfileId)
        {
            try
            {
                _logger.LogInformation(
                    "Retrieving beneficiaries for InvestorProfileId: {InvestorProfileId}",
                    investorProfileId);

                var response = await _beneficiaryService.GetByInvestorProfileIdAsync(investorProfileId);

                _logger.LogInformation(
                    "Retrieved {Count} beneficiaries for InvestorProfileId: {InvestorProfileId}",
                    response.Data?.Count ?? 0,
                    investorProfileId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error retrieving beneficiaries for InvestorProfileId: {InvestorProfileId}",
                    investorProfileId);

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while retrieving beneficiaries",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Retrieves beneficiaries grouped by type (Primary and Contingent) with percentage totals.
        /// Shows warning if percentage totals do not equal 100% for each type.
        /// Useful for validating beneficiary allocation and displaying grouped beneficiary information.
        /// </summary>
        /// <param name="investorProfileId">Investor profile ID</param>
        /// <returns>Grouped beneficiaries with totals and validation warnings</returns>
        /// <response code="200">Grouped beneficiaries retrieved successfully</response>
        /// <response code="401">Unauthorized - authentication required</response>
        [HttpGet("investor/{investorProfileId}/grouped")]
        [ProducesResponseType(typeof(ApiResponse<BeneficiariesGroupedResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<BeneficiariesGroupedResponse>>> GetBeneficiariesGrouped(
            int investorProfileId)
        {
            try
            {
                _logger.LogInformation(
                    "Retrieving grouped beneficiaries for InvestorProfileId: {InvestorProfileId}",
                    investorProfileId);

                var response = await _beneficiaryService.GetGroupedByInvestorProfileIdAsync(investorProfileId);

                _logger.LogInformation(
                    "Retrieved grouped beneficiaries for InvestorProfileId: {InvestorProfileId}, " +
                    "Primary Total: {PrimaryTotal}%, Contingent Total: {ContingentTotal}%",
                    investorProfileId,
                    response.Data?.PrimaryTotalPercentage ?? 0,
                    response.Data?.ContingentTotalPercentage ?? 0);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error retrieving grouped beneficiaries for InvestorProfileId: {InvestorProfileId}",
                    investorProfileId);

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while retrieving grouped beneficiaries",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}