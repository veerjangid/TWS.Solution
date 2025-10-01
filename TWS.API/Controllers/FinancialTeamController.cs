using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWS.Core.DTOs.Request.FinancialTeam;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.FinancialTeam;
using TWS.Core.Interfaces.IServices;

namespace TWS.API.Controllers
{
    /// <summary>
    /// Manages financial team member information for investor profiles.
    /// Handles CRUD operations for accountants, attorneys, financial advisors, insurance agents, and other team members.
    /// Reference: APIDoc.md Section 11
    /// </summary>
    [ApiController]
    [Route("api/financial-team")]
    [Authorize]
    public class FinancialTeamController : ControllerBase
    {
        private readonly IFinancialTeamService _financialTeamService;
        private readonly ILogger<FinancialTeamController> _logger;

        public FinancialTeamController(
            IFinancialTeamService financialTeamService,
            ILogger<FinancialTeamController> logger)
        {
            _financialTeamService = financialTeamService;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new financial team member to an investor profile.
        /// Supports 5 member types: Accountant, Attorney, FinancialAdvisor, InsuranceAgent, Other.
        /// </summary>
        /// <param name="request">Team member details including type, name, email, and phone</param>
        /// <returns>Created team member with assigned ID</returns>
        /// <response code="201">Team member successfully created</response>
        /// <response code="400">Validation failed or invalid member type</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="404">Investor profile not found</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<FinancialTeamMemberResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<FinancialTeamMemberResponse>>> AddMember(
            [FromBody] AddFinancialTeamMemberRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    _logger.LogWarning("Invalid model state for adding financial team member: {Errors}", string.Join(", ", errors));

                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = errors,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                _logger.LogInformation(
                    "Adding financial team member for InvestorProfileId: {InvestorProfileId}, MemberType: {MemberType}",
                    request.InvestorProfileId,
                    request.MemberType);

                var response = await _financialTeamService.AddMemberAsync(request);

                if (!response.Success)
                {
                    _logger.LogWarning(
                        "Failed to add financial team member: {Message}",
                        response.Message);

                    if (response.StatusCode == StatusCodes.Status404NotFound)
                        return NotFound(response);

                    return BadRequest(response);
                }

                _logger.LogInformation(
                    "Successfully added financial team member with ID: {MemberId}",
                    response.Data?.Id);

                return Created($"/api/financial-team/{response.Data?.Id}", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding financial team member for InvestorProfileId: {InvestorProfileId}",
                    request.InvestorProfileId);

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while adding the financial team member",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Updates an existing financial team member.
        /// Note: InvestorProfileId and MemberType cannot be changed after creation.
        /// </summary>
        /// <param name="id">The ID of the team member to update</param>
        /// <param name="request">Updated team member details (name, email, phone)</param>
        /// <returns>Updated team member details</returns>
        /// <response code="200">Team member successfully updated</response>
        /// <response code="400">Validation failed</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="404">Team member not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<FinancialTeamMemberResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<FinancialTeamMemberResponse>>> UpdateMember(
            int id,
            [FromBody] UpdateFinancialTeamMemberRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    _logger.LogWarning("Invalid model state for updating financial team member ID: {Id}: {Errors}",
                        id, string.Join(", ", errors));

                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = errors,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                _logger.LogInformation("Updating financial team member ID: {Id}", id);

                var response = await _financialTeamService.UpdateMemberAsync(id, request);

                if (!response.Success)
                {
                    _logger.LogWarning(
                        "Failed to update financial team member ID: {Id}: {Message}",
                        id,
                        response.Message);

                    if (response.StatusCode == StatusCodes.Status404NotFound)
                        return NotFound(response);

                    return BadRequest(response);
                }

                _logger.LogInformation(
                    "Successfully updated financial team member ID: {Id}",
                    id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating financial team member ID: {Id}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while updating the financial team member",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Deletes a financial team member by ID.
        /// </summary>
        /// <param name="id">The ID of the team member to delete</param>
        /// <returns>Success status</returns>
        /// <response code="200">Team member successfully deleted</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="404">Team member not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteMember(int id)
        {
            try
            {
                _logger.LogInformation("Deleting financial team member ID: {Id}", id);

                var response = await _financialTeamService.DeleteMemberAsync(id);

                if (!response.Success)
                {
                    _logger.LogWarning(
                        "Failed to delete financial team member ID: {Id}: {Message}",
                        id,
                        response.Message);

                    if (response.StatusCode == StatusCodes.Status404NotFound)
                        return NotFound(response);

                    return BadRequest(response);
                }

                _logger.LogInformation(
                    "Successfully deleted financial team member ID: {Id}",
                    id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting financial team member ID: {Id}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while deleting the financial team member",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Gets all financial team members for a specific investor profile.
        /// Returns members ordered by type, then name.
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <returns>List of team members</returns>
        /// <response code="200">Successfully retrieved team members</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="404">Investor profile not found</response>
        [HttpGet("investor/{investorProfileId}")]
        [ProducesResponseType(typeof(ApiResponse<List<FinancialTeamMemberResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<List<FinancialTeamMemberResponse>>>> GetByInvestorProfileId(
            int investorProfileId)
        {
            try
            {
                _logger.LogInformation(
                    "Retrieving financial team members for InvestorProfileId: {InvestorProfileId}",
                    investorProfileId);

                var response = await _financialTeamService.GetByInvestorProfileIdAsync(investorProfileId);

                if (!response.Success)
                {
                    _logger.LogWarning(
                        "Failed to retrieve financial team members for InvestorProfileId: {InvestorProfileId}: {Message}",
                        investorProfileId,
                        response.Message);

                    if (response.StatusCode == StatusCodes.Status404NotFound)
                        return NotFound(response);

                    return BadRequest(response);
                }

                _logger.LogInformation(
                    "Successfully retrieved {Count} financial team members for InvestorProfileId: {InvestorProfileId}",
                    response.Data?.Count ?? 0,
                    investorProfileId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error retrieving financial team members for InvestorProfileId: {InvestorProfileId}",
                    investorProfileId);

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while retrieving financial team members",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Gets a financial team member by ID.
        /// </summary>
        /// <param name="id">The team member ID</param>
        /// <returns>Team member details</returns>
        /// <response code="200">Successfully retrieved team member</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="404">Team member not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<FinancialTeamMemberResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<FinancialTeamMemberResponse>>> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving financial team member ID: {Id}", id);

                var response = await _financialTeamService.GetByIdAsync(id);

                if (!response.Success)
                {
                    _logger.LogWarning(
                        "Failed to retrieve financial team member ID: {Id}: {Message}",
                        id,
                        response.Message);

                    if (response.StatusCode == StatusCodes.Status404NotFound)
                        return NotFound(response);

                    return BadRequest(response);
                }

                _logger.LogInformation(
                    "Successfully retrieved financial team member ID: {Id}",
                    id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving financial team member ID: {Id}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the financial team member",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}