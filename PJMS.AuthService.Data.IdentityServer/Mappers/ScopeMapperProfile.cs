using AutoMapper;

namespace PJMS.AuthService.Data.IdentityServer.Mappers;

/// <summary>
/// Defines entity/model mapping for scopes.
/// </summary>
/// <seealso cref="AutoMapper.Profile" />
public class ScopeMapperProfile : Profile
{
    /// <summary>
    /// <see cref="ScopeMapperProfile"/>
    /// </summary>
    public ScopeMapperProfile()
    {
        CreateMap<Entities.ApiScopeProperty, KeyValuePair<string, string>>()
            .ReverseMap();

        CreateMap<Entities.ApiScopeClaim, string>()
            .ConstructUsing(x => x.Type)
            .ReverseMap()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

        CreateMap<Entities.ApiScope, IdentityServer4.Models.ApiScope>(MemberList.Destination)
            .ConstructUsing(src => new IdentityServer4.Models.ApiScope())
            .ForMember(x => x.Properties, opts => opts.MapFrom(x => x.Properties))
            .ForMember(x => x.UserClaims, opts => opts.MapFrom(x => x.UserClaims))
            .ReverseMap();
    }
}