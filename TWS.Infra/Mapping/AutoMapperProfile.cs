using AutoMapper;
using TWS.Core.DTOs.Request.Account;
using TWS.Core.DTOs.Request.GeneralInfo;
using TWS.Core.DTOs.Request.Investor;
using TWS.Core.DTOs.Response.Account;
using TWS.Core.DTOs.Response.Auth;
using TWS.Core.DTOs.Response.GeneralInfo;
using TWS.Core.DTOs.Response.Investor;
using TWS.Data.Entities.Core;
using TWS.Data.Entities.GeneralInfo;
using TWS.Data.Entities.Identity;
using TWS.Data.Entities.TypeSpecific;

namespace TWS.Infra.Mapping
{
    /// <summary>
    /// Base AutoMapper profile for configuring entity to DTO mappings
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Constructor to configure all mappings
        /// </summary>
        public AutoMapperProfile()
        {
            // AccountRequest mappings
            CreateMap<CreateAccountRequestRequest, AccountRequest>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RequestDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsProcessed, opt => opt.Ignore())
                .ForMember(dest => dest.ProcessedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ProcessedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.Notes, opt => opt.Ignore())
                .ForMember(dest => dest.ProcessedByUser, opt => opt.Ignore());

            CreateMap<AccountRequest, AccountRequestResponse>()
                .ForMember(dest => dest.ProcessedByUserName,
                    opt => opt.MapFrom(src => src.ProcessedByUser != null
                        ? src.ProcessedByUser.FullName
                        : null));

            // Authentication mappings
            CreateMap<ApplicationUser, LoginResponse>()
                .ForMember(dest => dest.Token, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
                .ForMember(dest => dest.ExpiresAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            CreateMap<ApplicationUser, RegisterResponse>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            // Investor Profile mappings - Request to Entity
            CreateMap<SelectInvestorTypeIndividualRequest, IndividualInvestorDetail>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfileId, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfile, opt => opt.Ignore());

            CreateMap<SelectInvestorTypeJointRequest, JointInvestorDetail>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfileId, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfile, opt => opt.Ignore());

            CreateMap<SelectInvestorTypeIRARequest, IRAInvestorDetail>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfileId, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfile, opt => opt.Ignore());

            CreateMap<SelectInvestorTypeTrustRequest, TrustInvestorDetail>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfileId, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfile, opt => opt.Ignore());

            CreateMap<SelectInvestorTypeEntityRequest, EntityInvestorDetail>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfileId, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfile, opt => opt.Ignore());

            // Investor Profile mappings - Entity to Response
            CreateMap<InvestorProfile, InvestorProfileResponse>()
                .ForMember(dest => dest.InvestorTypeName, opt => opt.MapFrom(src => src.InvestorType.ToString()))
                .ForMember(dest => dest.AccreditationTypeName, opt => opt.MapFrom(src => src.AccreditationType.HasValue ? src.AccreditationType.ToString() : null))
                .ForMember(dest => dest.TypeSpecificDetails, opt => opt.Ignore());

            CreateMap<IndividualInvestorDetail, InvestorProfileResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InvestorProfileId))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorType, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorTypeName, opt => opt.Ignore())
                .ForMember(dest => dest.IsAccredited, opt => opt.Ignore())
                .ForMember(dest => dest.AccreditationType, opt => opt.Ignore())
                .ForMember(dest => dest.AccreditationTypeName, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileCompletionPercentage, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.TypeSpecificDetails, opt => opt.MapFrom(src => src));

            CreateMap<JointInvestorDetail, InvestorProfileResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InvestorProfileId))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorType, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorTypeName, opt => opt.Ignore())
                .ForMember(dest => dest.IsAccredited, opt => opt.Ignore())
                .ForMember(dest => dest.AccreditationType, opt => opt.Ignore())
                .ForMember(dest => dest.AccreditationTypeName, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileCompletionPercentage, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.TypeSpecificDetails, opt => opt.MapFrom(src => src));

            CreateMap<IRAInvestorDetail, InvestorProfileResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InvestorProfileId))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorType, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorTypeName, opt => opt.Ignore())
                .ForMember(dest => dest.IsAccredited, opt => opt.Ignore())
                .ForMember(dest => dest.AccreditationType, opt => opt.Ignore())
                .ForMember(dest => dest.AccreditationTypeName, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileCompletionPercentage, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.TypeSpecificDetails, opt => opt.MapFrom(src => src));

            CreateMap<TrustInvestorDetail, InvestorProfileResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InvestorProfileId))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorType, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorTypeName, opt => opt.Ignore())
                .ForMember(dest => dest.IsAccredited, opt => opt.Ignore())
                .ForMember(dest => dest.AccreditationType, opt => opt.Ignore())
                .ForMember(dest => dest.AccreditationTypeName, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileCompletionPercentage, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.TypeSpecificDetails, opt => opt.MapFrom(src => src));

            CreateMap<EntityInvestorDetail, InvestorProfileResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InvestorProfileId))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorType, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorTypeName, opt => opt.Ignore())
                .ForMember(dest => dest.IsAccredited, opt => opt.Ignore())
                .ForMember(dest => dest.AccreditationType, opt => opt.Ignore())
                .ForMember(dest => dest.AccreditationTypeName, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileCompletionPercentage, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.TypeSpecificDetails, opt => opt.MapFrom(src => src));

            // General Info mappings - Request to Entity
            CreateMap<SaveIndividualGeneralInfoRequest, IndividualGeneralInfo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IndividualInvestorDetailId, opt => opt.Ignore())
                .ForMember(dest => dest.DriverLicensePath, opt => opt.Ignore())
                .ForMember(dest => dest.W9Path, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IndividualInvestorDetail, opt => opt.Ignore());

            CreateMap<SaveJointGeneralInfoRequest, JointGeneralInfo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.JointInvestorDetailId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.JointInvestorDetail, opt => opt.Ignore())
                .ForMember(dest => dest.JointAccountHolders, opt => opt.Ignore());

            CreateMap<AddJointAccountHolderRequest, JointAccountHolder>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.JointGeneralInfo, opt => opt.Ignore());

            CreateMap<SaveIRAGeneralInfoRequest, IRAGeneralInfo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IRAInvestorDetailId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IRAInvestorDetail, opt => opt.Ignore());

            CreateMap<SaveTrustGeneralInfoRequest, TrustGeneralInfo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TrustInvestorDetailId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.TrustInvestorDetail, opt => opt.Ignore())
                .ForMember(dest => dest.TrustGrantors, opt => opt.Ignore());

            CreateMap<AddTrustGrantorRequest, TrustGrantor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TrustGeneralInfo, opt => opt.Ignore());

            CreateMap<SaveEntityGeneralInfoRequest, EntityGeneralInfo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EntityInvestorDetailId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.EntityInvestorDetail, opt => opt.Ignore())
                .ForMember(dest => dest.EntityEquityOwners, opt => opt.Ignore());

            CreateMap<AddEntityEquityOwnerRequest, EntityEquityOwner>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EntityGeneralInfo, opt => opt.Ignore());

            // General Info mappings - Entity to Response
            CreateMap<IndividualGeneralInfo, GeneralInfoResponse>()
                .ForMember(dest => dest.InvestorProfileId, opt => opt.MapFrom(src => src.IndividualInvestorDetail.InvestorProfileId))
                .ForMember(dest => dest.InvestorType, opt => opt.MapFrom(src => (int)src.IndividualInvestorDetail.InvestorProfile.InvestorType))
                .ForMember(dest => dest.TypeSpecificData, opt => opt.MapFrom(src => src));

            CreateMap<JointGeneralInfo, GeneralInfoResponse>()
                .ForMember(dest => dest.InvestorProfileId, opt => opt.MapFrom(src => src.JointInvestorDetail.InvestorProfileId))
                .ForMember(dest => dest.InvestorType, opt => opt.MapFrom(src => (int)src.JointInvestorDetail.InvestorProfile.InvestorType))
                .ForMember(dest => dest.TypeSpecificData, opt => opt.MapFrom(src => src));

            CreateMap<IRAGeneralInfo, GeneralInfoResponse>()
                .ForMember(dest => dest.InvestorProfileId, opt => opt.MapFrom(src => src.IRAInvestorDetail.InvestorProfileId))
                .ForMember(dest => dest.InvestorType, opt => opt.MapFrom(src => (int)src.IRAInvestorDetail.InvestorProfile.InvestorType))
                .ForMember(dest => dest.TypeSpecificData, opt => opt.MapFrom(src => src));

            CreateMap<TrustGeneralInfo, GeneralInfoResponse>()
                .ForMember(dest => dest.InvestorProfileId, opt => opt.MapFrom(src => src.TrustInvestorDetail.InvestorProfileId))
                .ForMember(dest => dest.InvestorType, opt => opt.MapFrom(src => (int)src.TrustInvestorDetail.InvestorProfile.InvestorType))
                .ForMember(dest => dest.TypeSpecificData, opt => opt.MapFrom(src => src));

            CreateMap<EntityGeneralInfo, GeneralInfoResponse>()
                .ForMember(dest => dest.InvestorProfileId, opt => opt.MapFrom(src => src.EntityInvestorDetail.InvestorProfileId))
                .ForMember(dest => dest.InvestorType, opt => opt.MapFrom(src => (int)src.EntityInvestorDetail.InvestorProfile.InvestorType))
                .ForMember(dest => dest.TypeSpecificData, opt => opt.MapFrom(src => src));

            CreateMap<JointAccountHolder, JointAccountHolderResponse>()
                .ForMember(dest => dest.SSN, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.SSN) ? string.Empty : "***-**-" + src.SSN.Substring(Math.Max(0, src.SSN.Length - 4))));

            CreateMap<TrustGrantor, TrustGrantorResponse>();

            CreateMap<EntityEquityOwner, EntityEquityOwnerResponse>();
        }
    }
}