using AutoMapper;

namespace PJMS.AuthService.Data.IdentityServer.Mappers;

/// <summary>
/// Defines entity/model mapping for identity resources.
/// </summary>
/// <seealso cref="AutoMapper.Profile" />
public class IdentityResourceMapperProfile : Profile
{
    /// <summary>
    /// <see cref="IdentityResourceMapperProfile"/>
    /// </summary>
    public IdentityResourceMapperProfile()
    {
        CreateMap<Entities.IdentityResourceProperty, KeyValuePair<string, string>>()
            .ReverseMap();

        CreateMap<Entities.IdentityResource, IdentityServer4.Models.IdentityResource>(MemberList.Destination)
            .ConstructUsing(src => new IdentityServer4.Models.IdentityResource())
            .ReverseMap();

        CreateMap<Entities.IdentityResourceClaim, string>()
            .ConstructUsing(x => x.Type)
            .ReverseMap()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));
    }
}