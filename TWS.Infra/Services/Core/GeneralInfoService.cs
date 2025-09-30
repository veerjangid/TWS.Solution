using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TWS.Core.DTOs.Request.GeneralInfo;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.GeneralInfo;
using TWS.Core.Enums;
using TWS.Core.Interfaces.IServices;
using TWS.Data.Context;
using TWS.Data.Entities.GeneralInfo;

namespace TWS.Infra.Services.Core
{
    public class GeneralInfoService : IGeneralInfoService
    {
        private readonly TWSDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GeneralInfoService> _logger;

        public GeneralInfoService(
            TWSDbContext context,
            IMapper mapper,
            ILogger<GeneralInfoService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<GeneralInfoResponse>> SaveIndividualGeneralInfoAsync(SaveIndividualGeneralInfoRequest request)
        {
            try
            {
                _logger.LogInformation("Saving Individual General Info for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);

                var individualDetail = await _context.IndividualInvestorDetails
                    .Include(d => d.InvestorProfile)
                    .FirstOrDefaultAsync(d => d.InvestorProfileId == request.InvestorProfileId);

                if (individualDetail == null)
                {
                    _logger.LogWarning("Individual Investor Detail not found for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);
                    return ApiResponse<GeneralInfoResponse>.ErrorResponse("Individual investor detail not found", 404);
                }

                var existingGeneralInfo = await _context.IndividualGeneralInfos
                    .FirstOrDefaultAsync(g => g.IndividualInvestorDetailId == individualDetail.Id);

                IndividualGeneralInfo generalInfo;
                if (existingGeneralInfo != null)
                {
                    _logger.LogInformation("Updating existing Individual General Info with ID: {Id}", existingGeneralInfo.Id);
                    _mapper.Map(request, existingGeneralInfo);
                    existingGeneralInfo.UpdatedAt = DateTime.UtcNow;
                    generalInfo = existingGeneralInfo;
                }
                else
                {
                    _logger.LogInformation("Creating new Individual General Info");
                    generalInfo = _mapper.Map<IndividualGeneralInfo>(request);
                    generalInfo.IndividualInvestorDetailId = individualDetail.Id;
                    generalInfo.CreatedAt = DateTime.UtcNow;
                    _context.IndividualGeneralInfos.Add(generalInfo);
                }

                await _context.SaveChangesAsync();

                generalInfo = await _context.IndividualGeneralInfos
                    .Include(g => g.IndividualInvestorDetail)
                        .ThenInclude(d => d.InvestorProfile)
                    .FirstOrDefaultAsync(g => g.Id == generalInfo.Id) ?? generalInfo;

                var response = _mapper.Map<GeneralInfoResponse>(generalInfo);
                _logger.LogInformation("Successfully saved Individual General Info with ID: {Id}", generalInfo.Id);
                return ApiResponse<GeneralInfoResponse>.SuccessResponse(response, "Individual General Info saved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Individual General Info for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);
                return ApiResponse<GeneralInfoResponse>.ErrorResponse("An error occurred while saving Individual General Info", 500);
            }
        }

        public async Task<ApiResponse<GeneralInfoResponse>> SaveJointGeneralInfoAsync(SaveJointGeneralInfoRequest request)
        {
            try
            {
                _logger.LogInformation("Saving Joint General Info for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);

                var jointDetail = await _context.JointInvestorDetails
                    .Include(d => d.InvestorProfile)
                    .FirstOrDefaultAsync(d => d.InvestorProfileId == request.InvestorProfileId);

                if (jointDetail == null)
                {
                    _logger.LogWarning("Joint Investor Detail not found for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);
                    return ApiResponse<GeneralInfoResponse>.ErrorResponse("Joint investor detail not found", 404);
                }

                var existingGeneralInfo = await _context.JointGeneralInfos
                    .FirstOrDefaultAsync(g => g.JointInvestorDetailId == jointDetail.Id);

                JointGeneralInfo generalInfo;
                if (existingGeneralInfo != null)
                {
                    _logger.LogInformation("Updating existing Joint General Info with ID: {Id}", existingGeneralInfo.Id);
                    _mapper.Map(request, existingGeneralInfo);
                    existingGeneralInfo.UpdatedAt = DateTime.UtcNow;
                    generalInfo = existingGeneralInfo;
                }
                else
                {
                    _logger.LogInformation("Creating new Joint General Info");
                    generalInfo = _mapper.Map<JointGeneralInfo>(request);
                    generalInfo.JointInvestorDetailId = jointDetail.Id;
                    generalInfo.CreatedAt = DateTime.UtcNow;
                    _context.JointGeneralInfos.Add(generalInfo);
                }

                await _context.SaveChangesAsync();

                generalInfo = await _context.JointGeneralInfos
                    .Include(g => g.JointInvestorDetail)
                        .ThenInclude(d => d.InvestorProfile)
                    .Include(g => g.JointAccountHolders)
                    .FirstOrDefaultAsync(g => g.Id == generalInfo.Id) ?? generalInfo;

                var response = _mapper.Map<GeneralInfoResponse>(generalInfo);
                _logger.LogInformation("Successfully saved Joint General Info with ID: {Id}", generalInfo.Id);
                return ApiResponse<GeneralInfoResponse>.SuccessResponse(response, "Joint General Info saved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Joint General Info for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);
                return ApiResponse<GeneralInfoResponse>.ErrorResponse("An error occurred while saving Joint General Info", 500);
            }
        }

        public async Task<ApiResponse<JointAccountHolderResponse>> AddJointAccountHolderAsync(AddJointAccountHolderRequest request)
        {
            try
            {
                _logger.LogInformation("Adding Joint Account Holder to JointGeneralInfoId: {JointGeneralInfoId}", request.JointGeneralInfoId);

                var jointGeneralInfo = await _context.JointGeneralInfos.FirstOrDefaultAsync(g => g.Id == request.JointGeneralInfoId);
                if (jointGeneralInfo == null)
                {
                    _logger.LogWarning("Joint General Info not found with ID: {JointGeneralInfoId}", request.JointGeneralInfoId);
                    return ApiResponse<JointAccountHolderResponse>.ErrorResponse("Joint General Info not found", 404);
                }

                if (request.OrderIndex < 1)
                {
                    _logger.LogWarning("Invalid OrderIndex: {OrderIndex}", request.OrderIndex);
                    return ApiResponse<JointAccountHolderResponse>.ErrorResponse("OrderIndex must be 1 or greater", 400);
                }

                var accountHolder = _mapper.Map<JointAccountHolder>(request);
                accountHolder.CreatedAt = DateTime.UtcNow;
                _context.JointAccountHolders.Add(accountHolder);
                await _context.SaveChangesAsync();

                var response = _mapper.Map<JointAccountHolderResponse>(accountHolder);
                _logger.LogInformation("Successfully added Joint Account Holder with ID: {Id}", accountHolder.Id);
                return ApiResponse<JointAccountHolderResponse>.SuccessResponse(response, "Joint Account Holder added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Joint Account Holder to JointGeneralInfoId: {JointGeneralInfoId}", request.JointGeneralInfoId);
                return ApiResponse<JointAccountHolderResponse>.ErrorResponse("An error occurred while adding Joint Account Holder", 500);
            }
        }

        public async Task<ApiResponse<GeneralInfoResponse>> SaveIRAGeneralInfoAsync(SaveIRAGeneralInfoRequest request)
        {
            try
            {
                _logger.LogInformation("Saving IRA General Info for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);

                if (request.AccountType < 1 || request.AccountType > 5)
                {
                    _logger.LogWarning("Invalid IRA Account Type: {AccountType}", request.AccountType);
                    return ApiResponse<GeneralInfoResponse>.ErrorResponse("IRA Account Type must be one of the 5 valid types (1-5)", 400);
                }

                var iraDetail = await _context.IRAInvestorDetails
                    .Include(d => d.InvestorProfile)
                    .FirstOrDefaultAsync(d => d.InvestorProfileId == request.InvestorProfileId);

                if (iraDetail == null)
                {
                    _logger.LogWarning("IRA Investor Detail not found for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);
                    return ApiResponse<GeneralInfoResponse>.ErrorResponse("IRA investor detail not found", 404);
                }

                var existingGeneralInfo = await _context.IRAGeneralInfos
                    .FirstOrDefaultAsync(g => g.IRAInvestorDetailId == iraDetail.Id);

                IRAGeneralInfo generalInfo;
                if (existingGeneralInfo != null)
                {
                    _logger.LogInformation("Updating existing IRA General Info with ID: {Id}", existingGeneralInfo.Id);
                    _mapper.Map(request, existingGeneralInfo);
                    existingGeneralInfo.UpdatedAt = DateTime.UtcNow;
                    generalInfo = existingGeneralInfo;
                }
                else
                {
                    _logger.LogInformation("Creating new IRA General Info");
                    generalInfo = _mapper.Map<IRAGeneralInfo>(request);
                    generalInfo.IRAInvestorDetailId = iraDetail.Id;
                    generalInfo.CreatedAt = DateTime.UtcNow;
                    _context.IRAGeneralInfos.Add(generalInfo);
                }

                await _context.SaveChangesAsync();

                generalInfo = await _context.IRAGeneralInfos
                    .Include(g => g.IRAInvestorDetail)
                        .ThenInclude(d => d.InvestorProfile)
                    .FirstOrDefaultAsync(g => g.Id == generalInfo.Id) ?? generalInfo;

                var response = _mapper.Map<GeneralInfoResponse>(generalInfo);
                _logger.LogInformation("Successfully saved IRA General Info with ID: {Id}", generalInfo.Id);
                return ApiResponse<GeneralInfoResponse>.SuccessResponse(response, "IRA General Info saved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving IRA General Info for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);
                return ApiResponse<GeneralInfoResponse>.ErrorResponse("An error occurred while saving IRA General Info", 500);
            }
        }

        public async Task<ApiResponse<GeneralInfoResponse>> SaveTrustGeneralInfoAsync(SaveTrustGeneralInfoRequest request)
        {
            try
            {
                _logger.LogInformation("Saving Trust General Info for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);

                var trustDetail = await _context.TrustInvestorDetails
                    .Include(d => d.InvestorProfile)
                    .FirstOrDefaultAsync(d => d.InvestorProfileId == request.InvestorProfileId);

                if (trustDetail == null)
                {
                    _logger.LogWarning("Trust Investor Detail not found for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);
                    return ApiResponse<GeneralInfoResponse>.ErrorResponse("Trust investor detail not found", 404);
                }

                var existingGeneralInfo = await _context.TrustGeneralInfos
                    .FirstOrDefaultAsync(g => g.TrustInvestorDetailId == trustDetail.Id);

                TrustGeneralInfo generalInfo;
                if (existingGeneralInfo != null)
                {
                    _logger.LogInformation("Updating existing Trust General Info with ID: {Id}", existingGeneralInfo.Id);
                    _mapper.Map(request, existingGeneralInfo);
                    existingGeneralInfo.UpdatedAt = DateTime.UtcNow;
                    generalInfo = existingGeneralInfo;
                }
                else
                {
                    _logger.LogInformation("Creating new Trust General Info");
                    generalInfo = _mapper.Map<TrustGeneralInfo>(request);
                    generalInfo.TrustInvestorDetailId = trustDetail.Id;
                    generalInfo.CreatedAt = DateTime.UtcNow;
                    _context.TrustGeneralInfos.Add(generalInfo);
                }

                await _context.SaveChangesAsync();

                generalInfo = await _context.TrustGeneralInfos
                    .Include(g => g.TrustInvestorDetail)
                        .ThenInclude(d => d.InvestorProfile)
                    .Include(g => g.TrustGrantors)
                    .FirstOrDefaultAsync(g => g.Id == generalInfo.Id) ?? generalInfo;

                var response = _mapper.Map<GeneralInfoResponse>(generalInfo);
                _logger.LogInformation("Successfully saved Trust General Info with ID: {Id}", generalInfo.Id);
                return ApiResponse<GeneralInfoResponse>.SuccessResponse(response, "Trust General Info saved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Trust General Info for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);
                return ApiResponse<GeneralInfoResponse>.ErrorResponse("An error occurred while saving Trust General Info", 500);
            }
        }

        public async Task<ApiResponse<TrustGrantorResponse>> AddTrustGrantorAsync(AddTrustGrantorRequest request)
        {
            try
            {
                _logger.LogInformation("Adding Trust Grantor to TrustGeneralInfoId: {TrustGeneralInfoId}", request.TrustGeneralInfoId);

                var trustGeneralInfo = await _context.TrustGeneralInfos.FirstOrDefaultAsync(g => g.Id == request.TrustGeneralInfoId);
                if (trustGeneralInfo == null)
                {
                    _logger.LogWarning("Trust General Info not found with ID: {TrustGeneralInfoId}", request.TrustGeneralInfoId);
                    return ApiResponse<TrustGrantorResponse>.ErrorResponse("Trust General Info not found", 404);
                }

                var grantor = _mapper.Map<TrustGrantor>(request);
                _context.TrustGrantors.Add(grantor);
                await _context.SaveChangesAsync();

                var response = _mapper.Map<TrustGrantorResponse>(grantor);
                _logger.LogInformation("Successfully added Trust Grantor with ID: {Id}", grantor.Id);
                return ApiResponse<TrustGrantorResponse>.SuccessResponse(response, "Trust Grantor added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Trust Grantor to TrustGeneralInfoId: {TrustGeneralInfoId}", request.TrustGeneralInfoId);
                return ApiResponse<TrustGrantorResponse>.ErrorResponse("An error occurred while adding Trust Grantor", 500);
            }
        }

        public async Task<ApiResponse<GeneralInfoResponse>> SaveEntityGeneralInfoAsync(SaveEntityGeneralInfoRequest request)
        {
            try
            {
                _logger.LogInformation("Saving Entity General Info for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);

                var entityDetail = await _context.EntityInvestorDetails
                    .Include(d => d.InvestorProfile)
                    .FirstOrDefaultAsync(d => d.InvestorProfileId == request.InvestorProfileId);

                if (entityDetail == null)
                {
                    _logger.LogWarning("Entity Investor Detail not found for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);
                    return ApiResponse<GeneralInfoResponse>.ErrorResponse("Entity investor detail not found", 404);
                }

                var existingGeneralInfo = await _context.EntityGeneralInfos
                    .FirstOrDefaultAsync(g => g.EntityInvestorDetailId == entityDetail.Id);

                EntityGeneralInfo generalInfo;
                if (existingGeneralInfo != null)
                {
                    _logger.LogInformation("Updating existing Entity General Info with ID: {Id}", existingGeneralInfo.Id);
                    _mapper.Map(request, existingGeneralInfo);
                    existingGeneralInfo.UpdatedAt = DateTime.UtcNow;
                    generalInfo = existingGeneralInfo;
                }
                else
                {
                    _logger.LogInformation("Creating new Entity General Info");
                    generalInfo = _mapper.Map<EntityGeneralInfo>(request);
                    generalInfo.EntityInvestorDetailId = entityDetail.Id;
                    generalInfo.CreatedAt = DateTime.UtcNow;
                    _context.EntityGeneralInfos.Add(generalInfo);
                }

                await _context.SaveChangesAsync();

                generalInfo = await _context.EntityGeneralInfos
                    .Include(g => g.EntityInvestorDetail)
                        .ThenInclude(d => d.InvestorProfile)
                    .Include(g => g.EntityEquityOwners)
                    .FirstOrDefaultAsync(g => g.Id == generalInfo.Id) ?? generalInfo;

                var response = _mapper.Map<GeneralInfoResponse>(generalInfo);
                _logger.LogInformation("Successfully saved Entity General Info with ID: {Id}", generalInfo.Id);
                return ApiResponse<GeneralInfoResponse>.SuccessResponse(response, "Entity General Info saved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Entity General Info for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);
                return ApiResponse<GeneralInfoResponse>.ErrorResponse("An error occurred while saving Entity General Info", 500);
            }
        }

        public async Task<ApiResponse<EntityEquityOwnerResponse>> AddEntityEquityOwnerAsync(AddEntityEquityOwnerRequest request)
        {
            try
            {
                _logger.LogInformation("Adding Entity Equity Owner to EntityGeneralInfoId: {EntityGeneralInfoId}", request.EntityGeneralInfoId);

                var entityGeneralInfo = await _context.EntityGeneralInfos.FirstOrDefaultAsync(g => g.Id == request.EntityGeneralInfoId);
                if (entityGeneralInfo == null)
                {
                    _logger.LogWarning("Entity General Info not found with ID: {EntityGeneralInfoId}", request.EntityGeneralInfoId);
                    return ApiResponse<EntityEquityOwnerResponse>.ErrorResponse("Entity General Info not found", 404);
                }

                var equityOwner = _mapper.Map<EntityEquityOwner>(request);
                _context.EntityEquityOwners.Add(equityOwner);
                await _context.SaveChangesAsync();

                var response = _mapper.Map<EntityEquityOwnerResponse>(equityOwner);
                _logger.LogInformation("Successfully added Entity Equity Owner with ID: {Id}", equityOwner.Id);
                return ApiResponse<EntityEquityOwnerResponse>.SuccessResponse(response, "Entity Equity Owner added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Entity Equity Owner to EntityGeneralInfoId: {EntityGeneralInfoId}", request.EntityGeneralInfoId);
                return ApiResponse<EntityEquityOwnerResponse>.ErrorResponse("An error occurred while adding Entity Equity Owner", 500);
            }
        }

        public async Task<ApiResponse<GeneralInfoResponse>> GetGeneralInfoByInvestorProfileIdAsync(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Retrieving General Info for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                var investorProfile = await _context.InvestorProfiles.FirstOrDefaultAsync(p => p.Id == investorProfileId);
                if (investorProfile == null)
                {
                    _logger.LogWarning("Investor Profile not found with ID: {InvestorProfileId}", investorProfileId);
                    return ApiResponse<GeneralInfoResponse>.ErrorResponse("Investor profile not found", 404);
                }

                GeneralInfoResponse? response = null;

                switch (investorProfile.InvestorType)
                {
                    case InvestorType.Individual:
                        var individualDetail = await _context.IndividualInvestorDetails.FirstOrDefaultAsync(d => d.InvestorProfileId == investorProfileId);
                        if (individualDetail != null)
                        {
                            var individualGeneralInfo = await _context.IndividualGeneralInfos
                                .Include(g => g.IndividualInvestorDetail).ThenInclude(d => d.InvestorProfile)
                                .FirstOrDefaultAsync(g => g.IndividualInvestorDetailId == individualDetail.Id);
                            if (individualGeneralInfo != null) response = _mapper.Map<GeneralInfoResponse>(individualGeneralInfo);
                        }
                        break;

                    case InvestorType.Joint:
                        var jointDetail = await _context.JointInvestorDetails.FirstOrDefaultAsync(d => d.InvestorProfileId == investorProfileId);
                        if (jointDetail != null)
                        {
                            var jointGeneralInfo = await _context.JointGeneralInfos
                                .Include(g => g.JointInvestorDetail).ThenInclude(d => d.InvestorProfile)
                                .Include(g => g.JointAccountHolders)
                                .FirstOrDefaultAsync(g => g.JointInvestorDetailId == jointDetail.Id);
                            if (jointGeneralInfo != null) response = _mapper.Map<GeneralInfoResponse>(jointGeneralInfo);
                        }
                        break;

                    case InvestorType.IRA:
                        var iraDetail = await _context.IRAInvestorDetails.FirstOrDefaultAsync(d => d.InvestorProfileId == investorProfileId);
                        if (iraDetail != null)
                        {
                            var iraGeneralInfo = await _context.IRAGeneralInfos
                                .Include(g => g.IRAInvestorDetail).ThenInclude(d => d.InvestorProfile)
                                .FirstOrDefaultAsync(g => g.IRAInvestorDetailId == iraDetail.Id);
                            if (iraGeneralInfo != null) response = _mapper.Map<GeneralInfoResponse>(iraGeneralInfo);
                        }
                        break;

                    case InvestorType.Trust:
                        var trustDetail = await _context.TrustInvestorDetails.FirstOrDefaultAsync(d => d.InvestorProfileId == investorProfileId);
                        if (trustDetail != null)
                        {
                            var trustGeneralInfo = await _context.TrustGeneralInfos
                                .Include(g => g.TrustInvestorDetail).ThenInclude(d => d.InvestorProfile)
                                .Include(g => g.TrustGrantors)
                                .FirstOrDefaultAsync(g => g.TrustInvestorDetailId == trustDetail.Id);
                            if (trustGeneralInfo != null) response = _mapper.Map<GeneralInfoResponse>(trustGeneralInfo);
                        }
                        break;

                    case InvestorType.Entity:
                        var entityDetail = await _context.EntityInvestorDetails.FirstOrDefaultAsync(d => d.InvestorProfileId == investorProfileId);
                        if (entityDetail != null)
                        {
                            var entityGeneralInfo = await _context.EntityGeneralInfos
                                .Include(g => g.EntityInvestorDetail).ThenInclude(d => d.InvestorProfile)
                                .Include(g => g.EntityEquityOwners)
                                .FirstOrDefaultAsync(g => g.EntityInvestorDetailId == entityDetail.Id);
                            if (entityGeneralInfo != null) response = _mapper.Map<GeneralInfoResponse>(entityGeneralInfo);
                        }
                        break;
                }

                if (response == null)
                {
                    _logger.LogWarning("General Info not found for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                    return ApiResponse<GeneralInfoResponse>.ErrorResponse("General Info not found", 404);
                }

                _logger.LogInformation("Successfully retrieved General Info for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return ApiResponse<GeneralInfoResponse>.SuccessResponse(response, "General Info retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving General Info for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return ApiResponse<GeneralInfoResponse>.ErrorResponse("An error occurred while retrieving General Info", 500);
            }
        }
    }
}