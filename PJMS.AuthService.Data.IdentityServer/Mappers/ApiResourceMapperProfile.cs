using AutoMapper;

namespace PJMS.AuthService.Data.IdentityServer.Mappers;

/// <summary>
/// Defines entity/model mapping for API resources.
/// </summary>
/// <seealso cref="AutoMapper.Profile" />
public class ApiResourceMapperProfile : Profile
{
    /// <summary>
    /// <see cref="ApiResourceMapperProfile"/>
    /// </summary>
    public ApiResourceMapperProfile()
    {
        CreateMap<Entities.ApiResourceProperty, KeyValuePair<string, string>>()
            .ReverseMap();

        CreateMap<Entities.ApiResource, IdentityServer4.Models.ApiResource>(MemberList.Destination)
            .ConstructUsing(src => new IdentityServer4.Models.ApiResource())
            .ForMember(x => x.ApiSecrets, opts => opts.MapFrom(x => x.Secrets))
            .ForMember(x=>x.AllowedAccessTokenSigningAlgorithms, opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter, x=>x.AllowedAccessTokenSigningAlgorithms))
            .ReverseMap()
            .ForMember(x => x.AllowedAccessTokenSigningAlgorithms, opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter, x => x.AllowedAccessTokenSigningAlgorithms));

        CreateMap<Entities.ApiResourceClaim, string>()
            .ConstructUsing(x => x.Type)
            .ReverseMap()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

        CreateMap<Entities.ApiResourceSecret, IdentityServer4.Models.Secret>(MemberList.Destination)
            .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
            .ReverseMap();

        CreateMap<Entities.ApiResourceScope, string>()
            .ConstructUsing(x => x.Scope)
            .ReverseMap()
            .ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src));
    }
}