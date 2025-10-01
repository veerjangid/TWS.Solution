using AutoMapper;
using TWS.Core.DTOs.Request.Account;
using TWS.Core.DTOs.Request.Accreditation;
using TWS.Core.DTOs.Request.Beneficiary;
using TWS.Core.DTOs.Request.FinancialTeam;
using TWS.Core.DTOs.Request.GeneralInfo;
using TWS.Core.DTOs.Request.Investor;
using TWS.Core.DTOs.Request.Portal;
using TWS.Core.DTOs.Request.PrimaryInvestorInfo;
using TWS.Core.DTOs.Response.Account;
using TWS.Core.DTOs.Response.Accreditation;
using TWS.Core.DTOs.Response.Auth;
using TWS.Core.DTOs.Response.Beneficiary;
using TWS.Core.DTOs.Response.Document;
using TWS.Core.DTOs.Response.FinancialTeam;
using TWS.Core.DTOs.Response.GeneralInfo;
using TWS.Core.DTOs.Response.Investor;
using TWS.Core.DTOs.Response.PersonalFinancialStatement;
using TWS.Core.DTOs.Response.Portal;
using TWS.Core.DTOs.Response.PrimaryInvestorInfo;
using TWS.Core.DTOs.Response.Investment;
using TWS.Data.Entities.Accreditation;
using TWS.Data.Entities.Beneficiaries;
using TWS.Data.Entities.Core;
using TWS.Data.Entities.Documents;
using TWS.Data.Entities.Financial;
using TWS.Data.Entities.GeneralInfo;
using TWS.Data.Entities.Identity;
using TWS.Data.Entities.PrimaryInvestorInfo;
using TWS.Data.Entities.TypeSpecific;
using TWS.Data.Entities.Portal;

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

            // Primary Investor Info mappings - Request to Entity
            CreateMap<SavePrimaryInvestorInfoRequest, TWS.Data.Entities.PrimaryInvestorInfo.PrimaryInvestorInfo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfile, opt => opt.Ignore())
                .ForMember(dest => dest.BrokerAffiliations, opt => opt.Ignore())
                .ForMember(dest => dest.InvestmentExperiences, opt => opt.Ignore())
                .ForMember(dest => dest.SourceOfFunds, opt => opt.Ignore())
                .ForMember(dest => dest.TaxRates, opt => opt.Ignore());

            CreateMap<SaveBrokerAffiliationRequest, BrokerAffiliation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.PrimaryInvestorInfo, opt => opt.Ignore());

            CreateMap<InvestmentExperienceItem, InvestmentExperience>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PrimaryInvestorInfoId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.PrimaryInvestorInfo, opt => opt.Ignore());

            // Primary Investor Info mappings - Entity to Response
            CreateMap<TWS.Data.Entities.PrimaryInvestorInfo.PrimaryInvestorInfo, PrimaryInvestorInfoResponse>()
                .ForMember(dest => dest.SocialSecurityNumber, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.SocialSecurityNumber) ? string.Empty : "***-**-" + src.SocialSecurityNumber.Substring(Math.Max(0, src.SocialSecurityNumber.Length - 4))))
                .ForMember(dest => dest.BrokerAffiliation, opt => opt.MapFrom(src =>
                    src.BrokerAffiliations != null && src.BrokerAffiliations.Any() ? src.BrokerAffiliations.First() : null));

            CreateMap<BrokerAffiliation, BrokerAffiliationResponse>();

            CreateMap<InvestmentExperience, InvestmentExperienceResponse>()
                .ForMember(dest => dest.AssetClassName, opt => opt.MapFrom(src => ((TWS.Core.Enums.AssetClass)src.AssetClass).ToString()))
                .ForMember(dest => dest.ExperienceLevelName, opt => opt.MapFrom(src => ((TWS.Core.Enums.InvestmentExperienceLevel)src.ExperienceLevel).ToString()));

            CreateMap<SourceOfFunds, SourceOfFundsResponse>()
                .ForMember(dest => dest.SourceTypeName, opt => opt.MapFrom(src => ((TWS.Core.Enums.SourceOfFundsType)src.SourceType).ToString()));

            CreateMap<TaxRate, TaxRateResponse>()
                .ForMember(dest => dest.TaxRateRangeName, opt => opt.MapFrom(src => ((TWS.Core.Enums.TaxRateRange)src.TaxRateRange).ToString()));

            // Accreditation mappings - Request to Entity
            CreateMap<SaveAccreditationRequest, InvestorAccreditation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsVerified, opt => opt.Ignore())
                .ForMember(dest => dest.VerificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.VerifiedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.Notes, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfile, opt => opt.Ignore())
                .ForMember(dest => dest.VerifiedByUser, opt => opt.Ignore())
                .ForMember(dest => dest.AccreditationDocuments, opt => opt.Ignore());

            // Accreditation mappings - Entity to Response
            CreateMap<InvestorAccreditation, AccreditationResponse>()
                .ForMember(dest => dest.AccreditationTypeName, opt => opt.MapFrom(src => src.AccreditationType.ToString()))
                .ForMember(dest => dest.VerifiedByUserName, opt => opt.MapFrom(src => src.VerifiedByUser != null ? src.VerifiedByUser.FullName : null))
                .ForMember(dest => dest.Documents, opt => opt.MapFrom(src => src.AccreditationDocuments ?? new List<AccreditationDocument>()));

            CreateMap<AccreditationDocument, AccreditationDocumentResponse>();

            // Beneficiary mappings - Request to Entity
            CreateMap<AddBeneficiaryRequest, Beneficiary>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfile, opt => opt.Ignore());

            CreateMap<BeneficiaryItem, Beneficiary>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfileId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfile, opt => opt.Ignore());

            CreateMap<UpdateBeneficiaryRequest, Beneficiary>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfileId, opt => opt.Ignore())
                .ForMember(dest => dest.BeneficiaryType, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfile, opt => opt.Ignore());

            // Beneficiary mappings - Entity to Response
            CreateMap<Beneficiary, BeneficiaryResponse>()
                .ForMember(dest => dest.BeneficiaryTypeName, opt => opt.MapFrom(src => ((TWS.Core.Enums.BeneficiaryType)src.BeneficiaryType).ToString()))
                .ForMember(dest => dest.SocialSecurityNumber, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.SocialSecurityNumber) ? string.Empty : "***-**-" + src.SocialSecurityNumber.Substring(Math.Max(0, src.SocialSecurityNumber.Length - 4))));

            // Personal Financial Statement mappings - Entity to Response
            CreateMap<PersonalFinancialStatement, PFSResponse>();

            // Financial Goals mappings - Request to Entity
            CreateMap<TWS.Core.DTOs.Request.FinancialGoals.SaveFinancialGoalsRequest, FinancialGoals>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.LiquidityNeeds, opt => opt.MapFrom(src => (TWS.Core.Enums.LiquidityNeeds)src.LiquidityNeeds))
                .ForMember(dest => dest.InvestmentTimeline, opt => opt.MapFrom(src => (TWS.Core.Enums.InvestmentTimeline)src.InvestmentTimeline))
                .ForMember(dest => dest.InvestmentObjective, opt => opt.MapFrom(src => (TWS.Core.Enums.InvestmentObjective)src.InvestmentObjective))
                .ForMember(dest => dest.RiskTolerance, opt => opt.MapFrom(src => (TWS.Core.Enums.RiskTolerance)src.RiskTolerance))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfile, opt => opt.Ignore());

            // Financial Goals mappings - Entity to Response
            CreateMap<FinancialGoals, TWS.Core.DTOs.Response.FinancialGoals.FinancialGoalsResponse>()
                .ForMember(dest => dest.LiquidityNeeds, opt => opt.MapFrom(src => src.LiquidityNeeds.ToString()))
                .ForMember(dest => dest.InvestmentTimeline, opt => opt.MapFrom(src => src.InvestmentTimeline.ToString()))
                .ForMember(dest => dest.InvestmentObjective, opt => opt.MapFrom(src => src.InvestmentObjective.ToString()))
                .ForMember(dest => dest.RiskTolerance, opt => opt.MapFrom(src => src.RiskTolerance.ToString()));

            // Document mappings - Entity to Response
            CreateMap<InvestorDocument, DocumentResponse>();

            // Financial Team Member mappings - Request to Entity
            CreateMap<AddFinancialTeamMemberRequest, FinancialTeamMember>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MemberType, opt => opt.MapFrom(src => (TWS.Core.Enums.FinancialTeamMemberType)src.MemberType))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfile, opt => opt.Ignore());

            CreateMap<UpdateFinancialTeamMemberRequest, FinancialTeamMember>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfileId, opt => opt.Ignore())
                .ForMember(dest => dest.MemberType, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfile, opt => opt.Ignore());

            // Financial Team Member mappings - Entity to Response
            CreateMap<FinancialTeamMember, FinancialTeamMemberResponse>()
                .ForMember(dest => dest.MemberType, opt => opt.MapFrom(src => (int)src.MemberType))
                .ForMember(dest => dest.MemberTypeName, opt => opt.MapFrom(src => src.MemberType.ToString()));

            // Investment mappings - Entity to Response
            CreateMap<Offering, OfferingResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<OfferingDocument, OfferingDocumentResponse>();

            CreateMap<InvestorInvestment, InvestmentResponse>()
                .ForMember(dest => dest.OfferingName, opt => opt.MapFrom(src => src.Offering.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<InvestorInvestment, InvestmentDetailsResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Offering, opt => opt.MapFrom(src => src.Offering));

            // Portal/CRM InvestmentTracker mappings - Request to Entity
            CreateMap<CreateInvestmentTrackerRequest, InvestmentTracker>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Offering, opt => opt.Ignore())
                .ForMember(dest => dest.InvestorProfile, opt => opt.Ignore());

            // Portal/CRM InvestmentTracker mappings - Entity to Response
            CreateMap<InvestmentTracker, InvestmentTrackerResponse>()
                .ForMember(dest => dest.OfferingName, opt => opt.MapFrom(src => src.Offering.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.InvestmentType, opt => opt.MapFrom(src => src.InvestmentType.ToString()));

            CreateMap<InvestmentTracker, DashboardItemResponse>()
                .ForMember(dest => dest.OfferingName, opt => opt.MapFrom(src => src.Offering.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.InvestmentType, opt => opt.MapFrom(src => src.InvestmentType.ToString()));
        }
    }
}