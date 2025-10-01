using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Net;
using TWS.Core.DTOs.Request.FinancialTeam;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.FinancialTeam;
using TWS.Core.Enums;
using TWS.Core.Interfaces.IRepositories;
using TWS.Core.Interfaces.IServices;
using TWS.Data.Entities.Financial;

namespace TWS.Infra.Services.Financial
{
    /// <summary>
    /// Service implementation for Financial Team Member operations.
    /// Handles business logic for creating, retrieving, updating, and deleting financial team members.
    /// Reference: APIDoc.md Section 11, BusinessRequirement.md Section 12
    /// </summary>
    public class FinancialTeamService : IFinancialTeamService
    {
        private readonly IFinancialTeamMemberRepository _financialTeamMemberRepository;
        private readonly IInvestorProfileRepository _investorProfileRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FinancialTeamService> _logger;

        public FinancialTeamService(
            IFinancialTeamMemberRepository financialTeamMemberRepository,
            IInvestorProfileRepository investorProfileRepository,
            IMapper mapper,
            ILogger<FinancialTeamService> logger)
        {
            _financialTeamMemberRepository = financialTeamMemberRepository ?? throw new ArgumentNullException(nameof(financialTeamMemberRepository));
            _investorProfileRepository = investorProfileRepository ?? throw new ArgumentNullException(nameof(investorProfileRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Adds a new financial team member to an investor profile.
        /// </summary>
        public async Task<ApiResponse<FinancialTeamMemberResponse>> AddMemberAsync(AddFinancialTeamMemberRequest request)
        {
            try
            {
                _logger.LogInformation("Adding financial team member for InvestorProfileId: {InvestorProfileId}, MemberType: {MemberType}",
                    request.InvestorProfileId, request.MemberType);

                // Validate that the investor profile exists
                var investorProfile = await _investorProfileRepository.GetByIdAsync(request.InvestorProfileId);
                if (investorProfile == null)
                {
                    _logger.LogWarning("InvestorProfile not found for Id: {InvestorProfileId}", request.InvestorProfileId);
                    return ApiResponse<FinancialTeamMemberResponse>.ErrorResponse(
                        "Investor profile not found",
                        (int)HttpStatusCode.NotFound
                    );
                }

                // Validate MemberType enum value (1-5)
                if (!Enum.IsDefined(typeof(FinancialTeamMemberType), request.MemberType))
                {
                    _logger.LogWarning("Invalid MemberType: {MemberType}", request.MemberType);
                    return ApiResponse<FinancialTeamMemberResponse>.ErrorResponse(
                        "Invalid member type. Valid values are: 1=Accountant, 2=Attorney, 3=FinancialAdvisor, 4=InsuranceAgent, 5=Other",
                        (int)HttpStatusCode.BadRequest
                    );
                }

                // Create new financial team member entity
                var memberEntity = new FinancialTeamMember
                {
                    InvestorProfileId = request.InvestorProfileId,
                    MemberType = (FinancialTeamMemberType)request.MemberType,
                    Name = request.Name.Trim(),
                    Email = request.Email.Trim().ToLower(),
                    PhoneNumber = request.PhoneNumber.Trim(),
                    CreatedAt = DateTime.UtcNow
                };

                await _financialTeamMemberRepository.AddAsync(memberEntity);

                // Save changes
                var saved = await _financialTeamMemberRepository.SaveChangesAsync();
                if (!saved)
                {
                    _logger.LogError("Failed to add financial team member for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);
                    return ApiResponse<FinancialTeamMemberResponse>.ErrorResponse(
                        "Failed to add financial team member",
                        (int)HttpStatusCode.InternalServerError
                    );
                }

                var response = _mapper.Map<FinancialTeamMemberResponse>(memberEntity);

                _logger.LogInformation("Successfully added financial team member Id: {Id} for InvestorProfileId: {InvestorProfileId}",
                    memberEntity.Id, request.InvestorProfileId);

                return ApiResponse<FinancialTeamMemberResponse>.SuccessResponse(
                    response,
                    "Financial team member added successfully",
                    (int)HttpStatusCode.Created
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding financial team member for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);
                return ApiResponse<FinancialTeamMemberResponse>.ErrorResponse(
                    "An error occurred while adding financial team member",
                    (int)HttpStatusCode.InternalServerError
                );
            }
        }

        /// <summary>
        /// Updates an existing financial team member.
        /// Note: InvestorProfileId and MemberType cannot be changed after creation.
        /// </summary>
        public async Task<ApiResponse<FinancialTeamMemberResponse>> UpdateMemberAsync(int id, UpdateFinancialTeamMemberRequest request)
        {
            try
            {
                _logger.LogInformation("Updating financial team member Id: {Id}", id);

                // Retrieve existing team member
                var existingMember = await _financialTeamMemberRepository.GetByIdAsync(id);
                if (existingMember == null)
                {
                    _logger.LogWarning("Financial team member not found for Id: {Id}", id);
                    return ApiResponse<FinancialTeamMemberResponse>.ErrorResponse(
                        "Financial team member not found",
                        (int)HttpStatusCode.NotFound
                    );
                }

                var memberEntity = existingMember as FinancialTeamMember;
                if (memberEntity == null)
                {
                    _logger.LogError("Failed to cast financial team member entity for Id: {Id}", id);
                    return ApiResponse<FinancialTeamMemberResponse>.ErrorResponse(
                        "Internal error processing financial team member",
                        (int)HttpStatusCode.InternalServerError
                    );
                }

                // Update properties
                memberEntity.Name = request.Name.Trim();
                memberEntity.Email = request.Email.Trim().ToLower();
                memberEntity.PhoneNumber = request.PhoneNumber.Trim();
                memberEntity.UpdatedAt = DateTime.UtcNow;

                await _financialTeamMemberRepository.UpdateAsync(memberEntity);

                // Save changes
                var saved = await _financialTeamMemberRepository.SaveChangesAsync();
                if (!saved)
                {
                    _logger.LogError("Failed to update financial team member Id: {Id}", id);
                    return ApiResponse<FinancialTeamMemberResponse>.ErrorResponse(
                        "Failed to update financial team member",
                        (int)HttpStatusCode.InternalServerError
                    );
                }

                var response = _mapper.Map<FinancialTeamMemberResponse>(memberEntity);

                _logger.LogInformation("Successfully updated financial team member Id: {Id}", id);

                return ApiResponse<FinancialTeamMemberResponse>.SuccessResponse(
                    response,
                    "Financial team member updated successfully",
                    (int)HttpStatusCode.OK
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating financial team member Id: {Id}", id);
                return ApiResponse<FinancialTeamMemberResponse>.ErrorResponse(
                    "An error occurred while updating financial team member",
                    (int)HttpStatusCode.InternalServerError
                );
            }
        }

        /// <summary>
        /// Deletes a financial team member by ID.
        /// </summary>
        public async Task<ApiResponse<bool>> DeleteMemberAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting financial team member Id: {Id}", id);

                // Retrieve existing team member
                var existingMember = await _financialTeamMemberRepository.GetByIdAsync(id);
                if (existingMember == null)
                {
                    _logger.LogWarning("Financial team member not found for Id: {Id}", id);
                    return ApiResponse<bool>.ErrorResponse(
                        "Financial team member not found",
                        (int)HttpStatusCode.NotFound
                    );
                }

                await _financialTeamMemberRepository.DeleteAsync(existingMember);

                // Save changes
                var saved = await _financialTeamMemberRepository.SaveChangesAsync();
                if (!saved)
                {
                    _logger.LogError("Failed to delete financial team member Id: {Id}", id);
                    return ApiResponse<bool>.ErrorResponse(
                        "Failed to delete financial team member",
                        (int)HttpStatusCode.InternalServerError
                    );
                }

                _logger.LogInformation("Successfully deleted financial team member Id: {Id}", id);

                return ApiResponse<bool>.SuccessResponse(
                    true,
                    "Financial team member deleted successfully",
                    (int)HttpStatusCode.OK
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting financial team member Id: {Id}", id);
                return ApiResponse<bool>.ErrorResponse(
                    "An error occurred while deleting financial team member",
                    (int)HttpStatusCode.InternalServerError
                );
            }
        }

        /// <summary>
        /// Gets all financial team members for an investor profile.
        /// </summary>
        public async Task<ApiResponse<List<FinancialTeamMemberResponse>>> GetByInvestorProfileIdAsync(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Retrieving financial team members for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                // Validate that the investor profile exists
                var investorProfile = await _investorProfileRepository.GetByIdAsync(investorProfileId);
                if (investorProfile == null)
                {
                    _logger.LogWarning("InvestorProfile not found for Id: {InvestorProfileId}", investorProfileId);
                    return ApiResponse<List<FinancialTeamMemberResponse>>.ErrorResponse(
                        "Investor profile not found",
                        (int)HttpStatusCode.NotFound
                    );
                }

                var members = await _financialTeamMemberRepository.GetByInvestorProfileIdAsync(investorProfileId);
                var membersList = members.Cast<FinancialTeamMember>().ToList();

                var response = _mapper.Map<List<FinancialTeamMemberResponse>>(membersList);

                _logger.LogInformation("Successfully retrieved {Count} financial team members for InvestorProfileId: {InvestorProfileId}",
                    response.Count, investorProfileId);

                return ApiResponse<List<FinancialTeamMemberResponse>>.SuccessResponse(
                    response,
                    "Financial team members retrieved successfully",
                    (int)HttpStatusCode.OK
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving financial team members for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return ApiResponse<List<FinancialTeamMemberResponse>>.ErrorResponse(
                    "An error occurred while retrieving financial team members",
                    (int)HttpStatusCode.InternalServerError
                );
            }
        }

        /// <summary>
        /// Gets a financial team member by ID.
        /// </summary>
        public async Task<ApiResponse<FinancialTeamMemberResponse>> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving financial team member Id: {Id}", id);

                var member = await _financialTeamMemberRepository.GetByIdAsync(id);
                if (member == null)
                {
                    _logger.LogWarning("Financial team member not found for Id: {Id}", id);
                    return ApiResponse<FinancialTeamMemberResponse>.ErrorResponse(
                        "Financial team member not found",
                        (int)HttpStatusCode.NotFound
                    );
                }

                var memberEntity = member as FinancialTeamMember;
                if (memberEntity == null)
                {
                    _logger.LogError("Failed to cast financial team member entity for Id: {Id}", id);
                    return ApiResponse<FinancialTeamMemberResponse>.ErrorResponse(
                        "Internal error processing financial team member",
                        (int)HttpStatusCode.InternalServerError
                    );
                }

                var response = _mapper.Map<FinancialTeamMemberResponse>(memberEntity);

                _logger.LogInformation("Successfully retrieved financial team member Id: {Id}", id);

                return ApiResponse<FinancialTeamMemberResponse>.SuccessResponse(
                    response,
                    "Financial team member retrieved successfully",
                    (int)HttpStatusCode.OK
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving financial team member Id: {Id}", id);
                return ApiResponse<FinancialTeamMemberResponse>.ErrorResponse(
                    "An error occurred while retrieving financial team member",
                    (int)HttpStatusCode.InternalServerError
                );
            }
        }
    }
}