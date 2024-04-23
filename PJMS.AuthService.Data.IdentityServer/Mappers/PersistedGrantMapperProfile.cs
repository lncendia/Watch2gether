using AutoMapper;

namespace PJMS.AuthService.Data.IdentityServer.Mappers;

/// <summary>
/// Defines entity/model mapping for persisted grants.
/// </summary>
/// <seealso cref="AutoMapper.Profile" />
public class PersistedGrantMapperProfile:Profile
{
    /// <summary>
    /// <see cref="PersistedGrantMapperProfile">
    /// </see>
    /// </summary>
    public PersistedGrantMapperProfile()
    {
        CreateMap<Entities.PersistedGrant, IdentityServer4.Models.PersistedGrant>(MemberList.Destination)
            .ReverseMap();
    }
}