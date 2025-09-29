using AutoMapper;
using TWS.Core.DTOs.Request.Account;
using TWS.Core.DTOs.Response.Account;
using TWS.Core.DTOs.Response.Auth;
using TWS.Data.Entities.Core;
using TWS.Data.Entities.Identity;

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
        }
    }
}