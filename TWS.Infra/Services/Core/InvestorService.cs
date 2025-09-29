using AutoMapper;
using Microsoft.Extensions.Logging;
using TWS.Core.DTOs.Request.Investor;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Investor;
using TWS.Core.Enums;
using TWS.Core.Interfaces.IRepositories;
using TWS.Core.Interfaces.IServices;
using TWS.Data.Entities.Core;
using TWS.Data.Entities.TypeSpecific;
using TWS.Data.Repositories.TypeSpecific;

namespace TWS.Infra.Services.Core
{
    /// <summary>
    /// Service implementation for investor profile operations
    /// </summary>
    public class InvestorService : IInvestorService
    {
        private readonly IInvestorProfileRepository _investorProfileRepository;
        private readonly IndividualInvestorDetailRepository _individualRepository;
        private readonly JointInvestorDetailRepository _jointRepository;
        private readonly IRAInvestorDetailRepository _iraRepository;
        private readonly TrustInvestorDetailRepository _trustRepository;
        private readonly EntityInvestorDetailRepository _entityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InvestorService> _logger;

        public InvestorService(
            IInvestorProfileRepository investorProfileRepository,
            IndividualInvestorDetailRepository individualRepository,
            JointInvestorDetailRepository jointRepository,
            IRAInvestorDetailRepository iraRepository,
            TrustInvestorDetailRepository trustRepository,
            EntityInvestorDetailRepository entityRepository,
            IMapper mapper,
            ILogger<InvestorService> logger)
        {
            _investorProfileRepository = investorProfileRepository;
            _individualRepository = individualRepository;
            _jointRepository = jointRepository;
            _iraRepository = iraRepository;
            _trustRepository = trustRepository;
            _entityRepository = entityRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<InvestorProfileResponse>> SelectInvestorTypeIndividualAsync(string userId, SelectInvestorTypeIndividualRequest request)
        {
            try
            {
                _logger.LogInformation("Creating Individual investor profile for user {UserId}", userId);

                // Check if user already has a profile
                var existingProfile = await _investorProfileRepository.HasProfileAsync(userId);
                if (existingProfile)
                {
                    _logger.LogWarning("User {UserId} already has an investor profile", userId);
                    return ApiResponse<InvestorProfileResponse>.ErrorResponse("User already has an investor profile", 400);
                }

                // Validate accreditation type if accredited
                if (request.IsAccredited && !request.AccreditationType.HasValue)
                {
                    _logger.LogWarning("Accreditation type is required when IsAccredited is true");
                    return ApiResponse<InvestorProfileResponse>.ErrorResponse("Accreditation type is required when investor is accredited", 400);
                }

                // Create InvestorProfile
                var investorProfile = new InvestorProfile
                {
                    UserId = userId,
                    InvestorType = InvestorType.Individual,
                    IsAccredited = request.IsAccredited,
                    AccreditationType = request.AccreditationType.HasValue ? (AccreditationType)request.AccreditationType.Value : null,
                    ProfileCompletionPercentage = 10,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _investorProfileRepository.AddAsync(investorProfile);
                await _investorProfileRepository.SaveChangesAsync();

                // Create Individual-specific details
                var individualDetail = _mapper.Map<IndividualInvestorDetail>(request);
                individualDetail.InvestorProfileId = investorProfile.Id;

                await _individualRepository.AddAsync(individualDetail);
                await _individualRepository.SaveChangesAsync();

                _logger.LogInformation("Individual investor profile created successfully for user {UserId}", userId);

                // Map to response
                var response = _mapper.Map<InvestorProfileResponse>(investorProfile);
                response.TypeSpecificDetails = individualDetail;

                return ApiResponse<InvestorProfileResponse>.SuccessResponse(response, "Individual investor profile created successfully", 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Individual investor profile for user {UserId}", userId);
                return ApiResponse<InvestorProfileResponse>.ErrorResponse($"Error creating investor profile: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<InvestorProfileResponse>> SelectInvestorTypeJointAsync(string userId, SelectInvestorTypeJointRequest request)
        {
            try
            {
                _logger.LogInformation("Creating Joint investor profile for user {UserId}", userId);

                // Check if user already has a profile
                var existingProfile = await _investorProfileRepository.HasProfileAsync(userId);
                if (existingProfile)
                {
                    _logger.LogWarning("User {UserId} already has an investor profile", userId);
                    return ApiResponse<InvestorProfileResponse>.ErrorResponse("User already has an investor profile", 400);
                }

                // Validate IsJointInvestment is true
                if (!request.IsJointInvestment)
                {
                    _logger.LogWarning("IsJointInvestment must be true for Joint investor type");
                    return ApiResponse<InvestorProfileResponse>.ErrorResponse("IsJointInvestment must be true for Joint investor type", 400);
                }

                // Validate accreditation type if accredited
                if (request.IsAccredited && !request.AccreditationType.HasValue)
                {
                    _logger.LogWarning("Accreditation type is required when IsAccredited is true");
                    return ApiResponse<InvestorProfileResponse>.ErrorResponse("Accreditation type is required when investor is accredited", 400);
                }

                // Create InvestorProfile
                var investorProfile = new InvestorProfile
                {
                    UserId = userId,
                    InvestorType = InvestorType.Joint,
                    IsAccredited = request.IsAccredited,
                    AccreditationType = request.AccreditationType.HasValue ? (AccreditationType)request.AccreditationType.Value : null,
                    ProfileCompletionPercentage = 10,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _investorProfileRepository.AddAsync(investorProfile);
                await _investorProfileRepository.SaveChangesAsync();

                // Create Joint-specific details
                var jointDetail = _mapper.Map<JointInvestorDetail>(request);
                jointDetail.InvestorProfileId = investorProfile.Id;

                await _jointRepository.AddAsync(jointDetail);
                await _jointRepository.SaveChangesAsync();

                _logger.LogInformation("Joint investor profile created successfully for user {UserId}", userId);

                // Map to response
                var response = _mapper.Map<InvestorProfileResponse>(investorProfile);
                response.TypeSpecificDetails = jointDetail;

                return ApiResponse<InvestorProfileResponse>.SuccessResponse(response, "Joint investor profile created successfully", 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Joint investor profile for user {UserId}", userId);
                return ApiResponse<InvestorProfileResponse>.ErrorResponse($"Error creating investor profile: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<InvestorProfileResponse>> SelectInvestorTypeIRAAsync(string userId, SelectInvestorTypeIRARequest request)
        {
            try
            {
                _logger.LogInformation("Creating IRA investor profile for user {UserId}", userId);

                // Check if user already has a profile
                var existingProfile = await _investorProfileRepository.HasProfileAsync(userId);
                if (existingProfile)
                {
                    _logger.LogWarning("User {UserId} already has an investor profile", userId);
                    return ApiResponse<InvestorProfileResponse>.ErrorResponse("User already has an investor profile", 400);
                }

                // Validate IRA type is one of the 5 valid types
                if (!Enum.IsDefined(typeof(IRAAccountType), request.IRAType))
                {
                    _logger.LogWarning("Invalid IRA type: {IRAType}", request.IRAType);
                    return ApiResponse<InvestorProfileResponse>.ErrorResponse("Invalid IRA type", 400);
                }

                // Validate accreditation type if accredited
                if (request.IsAccredited && !request.AccreditationType.HasValue)
                {
                    _logger.LogWarning("Accreditation type is required when IsAccredited is true");
                    return ApiResponse<InvestorProfileResponse>.ErrorResponse("Accreditation type is required when investor is accredited", 400);
                }

                // Create InvestorProfile
                var investorProfile = new InvestorProfile
                {
                    UserId = userId,
                    InvestorType = InvestorType.IRA,
                    IsAccredited = request.IsAccredited,
                    AccreditationType = request.AccreditationType.HasValue ? (AccreditationType)request.AccreditationType.Value : null,
                    ProfileCompletionPercentage = 10,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _investorProfileRepository.AddAsync(investorProfile);
                await _investorProfileRepository.SaveChangesAsync();

                // Create IRA-specific details
                var iraDetail = _mapper.Map<IRAInvestorDetail>(request);
                iraDetail.InvestorProfileId = investorProfile.Id;

                await _iraRepository.AddAsync(iraDetail);
                await _iraRepository.SaveChangesAsync();

                _logger.LogInformation("IRA investor profile created successfully for user {UserId}", userId);

                // Map to response
                var response = _mapper.Map<InvestorProfileResponse>(investorProfile);
                response.TypeSpecificDetails = iraDetail;

                return ApiResponse<InvestorProfileResponse>.SuccessResponse(response, "IRA investor profile created successfully", 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating IRA investor profile for user {UserId}", userId);
                return ApiResponse<InvestorProfileResponse>.ErrorResponse($"Error creating investor profile: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<InvestorProfileResponse>> SelectInvestorTypeTrustAsync(string userId, SelectInvestorTypeTrustRequest request)
        {
            try
            {
                _logger.LogInformation("Creating Trust investor profile for user {UserId}", userId);

                // Check if user already has a profile
                var existingProfile = await _investorProfileRepository.HasProfileAsync(userId);
                if (existingProfile)
                {
                    _logger.LogWarning("User {UserId} already has an investor profile", userId);
                    return ApiResponse<InvestorProfileResponse>.ErrorResponse("User already has an investor profile", 400);
                }

                // Validate accreditation type if accredited
                if (request.IsAccredited && !request.AccreditationType.HasValue)
                {
                    _logger.LogWarning("Accreditation type is required when IsAccredited is true");
                    return ApiResponse<InvestorProfileResponse>.ErrorResponse("Accreditation type is required when investor is accredited", 400);
                }

                // Create InvestorProfile
                var investorProfile = new InvestorProfile
                {
                    UserId = userId,
                    InvestorType = InvestorType.Trust,
                    IsAccredited = request.IsAccredited,
                    AccreditationType = request.AccreditationType.HasValue ? (AccreditationType)request.AccreditationType.Value : null,
                    ProfileCompletionPercentage = 10,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _investorProfileRepository.AddAsync(investorProfile);
                await _investorProfileRepository.SaveChangesAsync();

                // Create Trust-specific details
                var trustDetail = _mapper.Map<TrustInvestorDetail>(request);
                trustDetail.InvestorProfileId = investorProfile.Id;

                await _trustRepository.AddAsync(trustDetail);
                await _trustRepository.SaveChangesAsync();

                _logger.LogInformation("Trust investor profile created successfully for user {UserId}", userId);

                // Map to response
                var response = _mapper.Map<InvestorProfileResponse>(investorProfile);
                response.TypeSpecificDetails = trustDetail;

                return ApiResponse<InvestorProfileResponse>.SuccessResponse(response, "Trust investor profile created successfully", 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Trust investor profile for user {UserId}", userId);
                return ApiResponse<InvestorProfileResponse>.ErrorResponse($"Error creating investor profile: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<InvestorProfileResponse>> SelectInvestorTypeEntityAsync(string userId, SelectInvestorTypeEntityRequest request)
        {
            try
            {
                _logger.LogInformation("Creating Entity investor profile for user {UserId}", userId);

                // Check if user already has a profile
                var existingProfile = await _investorProfileRepository.HasProfileAsync(userId);
                if (existingProfile)
                {
                    _logger.LogWarning("User {UserId} already has an investor profile", userId);
                    return ApiResponse<InvestorProfileResponse>.ErrorResponse("User already has an investor profile", 400);
                }

                // Validate accreditation type if accredited
                if (request.IsAccredited && !request.AccreditationType.HasValue)
                {
                    _logger.LogWarning("Accreditation type is required when IsAccredited is true");
                    return ApiResponse<InvestorProfileResponse>.ErrorResponse("Accreditation type is required when investor is accredited", 400);
                }

                // Create InvestorProfile
                var investorProfile = new InvestorProfile
                {
                    UserId = userId,
                    InvestorType = InvestorType.Entity,
                    IsAccredited = request.IsAccredited,
                    AccreditationType = request.AccreditationType.HasValue ? (AccreditationType)request.AccreditationType.Value : null,
                    ProfileCompletionPercentage = 10,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _investorProfileRepository.AddAsync(investorProfile);
                await _investorProfileRepository.SaveChangesAsync();

                // Create Entity-specific details
                var entityDetail = _mapper.Map<EntityInvestorDetail>(request);
                entityDetail.InvestorProfileId = investorProfile.Id;

                await _entityRepository.AddAsync(entityDetail);
                await _entityRepository.SaveChangesAsync();

                _logger.LogInformation("Entity investor profile created successfully for user {UserId}", userId);

                // Map to response
                var response = _mapper.Map<InvestorProfileResponse>(investorProfile);
                response.TypeSpecificDetails = entityDetail;

                return ApiResponse<InvestorProfileResponse>.SuccessResponse(response, "Entity investor profile created successfully", 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Entity investor profile for user {UserId}", userId);
                return ApiResponse<InvestorProfileResponse>.ErrorResponse($"Error creating investor profile: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<InvestorProfileResponse>> GetInvestorProfileAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Retrieving investor profile for user {UserId}", userId);

                var profileObj = await _investorProfileRepository.GetByUserIdAsync(userId);
                if (profileObj == null)
                {
                    _logger.LogWarning("Investor profile not found for user {UserId}", userId);
                    return ApiResponse<InvestorProfileResponse>.ErrorResponse("Investor profile not found", 404);
                }

                var profile = (InvestorProfile)profileObj;

                // Map base profile to response
                var response = _mapper.Map<InvestorProfileResponse>(profile);

                // Load type-specific details based on investor type
                object? typeSpecificDetails = null;
                switch (profile.InvestorType)
                {
                    case InvestorType.Individual:
                        typeSpecificDetails = await _individualRepository.FindFirstAsync(d => d.InvestorProfileId == profile.Id);
                        break;
                    case InvestorType.Joint:
                        typeSpecificDetails = await _jointRepository.FindFirstAsync(d => d.InvestorProfileId == profile.Id);
                        break;
                    case InvestorType.IRA:
                        typeSpecificDetails = await _iraRepository.FindFirstAsync(d => d.InvestorProfileId == profile.Id);
                        break;
                    case InvestorType.Trust:
                        typeSpecificDetails = await _trustRepository.FindFirstAsync(d => d.InvestorProfileId == profile.Id);
                        break;
                    case InvestorType.Entity:
                        typeSpecificDetails = await _entityRepository.FindFirstAsync(d => d.InvestorProfileId == profile.Id);
                        break;
                }

                response.TypeSpecificDetails = typeSpecificDetails;

                _logger.LogInformation("Investor profile retrieved successfully for user {UserId}", userId);
                return ApiResponse<InvestorProfileResponse>.SuccessResponse(response, "Investor profile retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving investor profile for user {UserId}", userId);
                return ApiResponse<InvestorProfileResponse>.ErrorResponse($"Error retrieving investor profile: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<InvestorProfileResponse>> GetInvestorProfileByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving investor profile by ID {ProfileId}", id);

                var profileObj = await _investorProfileRepository.GetByIdAsync(id);
                if (profileObj == null)
                {
                    _logger.LogWarning("Investor profile not found with ID {ProfileId}", id);
                    return ApiResponse<InvestorProfileResponse>.ErrorResponse("Investor profile not found", 404);
                }

                var profile = (InvestorProfile)profileObj;

                // Map base profile to response
                var response = _mapper.Map<InvestorProfileResponse>(profile);

                // Load type-specific details based on investor type
                object? typeSpecificDetails = null;
                switch (profile.InvestorType)
                {
                    case InvestorType.Individual:
                        typeSpecificDetails = await _individualRepository.FindFirstAsync(d => d.InvestorProfileId == profile.Id);
                        break;
                    case InvestorType.Joint:
                        typeSpecificDetails = await _jointRepository.FindFirstAsync(d => d.InvestorProfileId == profile.Id);
                        break;
                    case InvestorType.IRA:
                        typeSpecificDetails = await _iraRepository.FindFirstAsync(d => d.InvestorProfileId == profile.Id);
                        break;
                    case InvestorType.Trust:
                        typeSpecificDetails = await _trustRepository.FindFirstAsync(d => d.InvestorProfileId == profile.Id);
                        break;
                    case InvestorType.Entity:
                        typeSpecificDetails = await _entityRepository.FindFirstAsync(d => d.InvestorProfileId == profile.Id);
                        break;
                }

                response.TypeSpecificDetails = typeSpecificDetails;

                _logger.LogInformation("Investor profile retrieved successfully by ID {ProfileId}", id);
                return ApiResponse<InvestorProfileResponse>.SuccessResponse(response, "Investor profile retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving investor profile by ID {ProfileId}", id);
                return ApiResponse<InvestorProfileResponse>.ErrorResponse($"Error retrieving investor profile: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdateAccreditationAsync(int investorProfileId, bool isAccredited, int? accreditationType)
        {
            try
            {
                _logger.LogInformation("Updating accreditation status for profile {ProfileId}", investorProfileId);

                var profileObj = await _investorProfileRepository.GetByIdAsync(investorProfileId);
                if (profileObj == null)
                {
                    _logger.LogWarning("Investor profile not found with ID {ProfileId}", investorProfileId);
                    return ApiResponse<bool>.ErrorResponse("Investor profile not found", 404);
                }

                var profile = (InvestorProfile)profileObj;

                // Validate accreditation type if accredited
                if (isAccredited && !accreditationType.HasValue)
                {
                    _logger.LogWarning("Accreditation type is required when IsAccredited is true");
                    return ApiResponse<bool>.ErrorResponse("Accreditation type is required when investor is accredited", 400);
                }

                profile.IsAccredited = isAccredited;
                profile.AccreditationType = accreditationType.HasValue ? (AccreditationType)accreditationType.Value : null;
                profile.UpdatedAt = DateTime.UtcNow;

                await _investorProfileRepository.UpdateAsync(profile);
                await _investorProfileRepository.SaveChangesAsync();

                _logger.LogInformation("Accreditation status updated successfully for profile {ProfileId}", investorProfileId);
                return ApiResponse<bool>.SuccessResponse(true, "Accreditation status updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating accreditation status for profile {ProfileId}", investorProfileId);
                return ApiResponse<bool>.ErrorResponse($"Error updating accreditation status: {ex.Message}", 500);
            }
        }
    }
}