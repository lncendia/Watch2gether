using AutoMapper;
using PJMS.AuthService.Data.IdentityServer.Entities;

namespace PJMS.AuthService.Data.IdentityServer.Mappers;

/// <summary>
/// Extension methods to map to/from entity/model for scopes.
/// </summary>
public static class ScopeMappers
{
    static ScopeMappers()
    {
        Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ScopeMapperProfile>())
            .CreateMapper();
    }

    private static IMapper Mapper { get; }

    /// <summary>
    /// Maps an entity to a model.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    public static IdentityServer4.Models.ApiScope ToModel(this ApiScope entity)
    {
        return entity == null ? null : Mapper.Map<IdentityServer4.Models.ApiScope>(entity);
    }

    /// <summary>
    /// Maps a model to an entity.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    public static ApiScope ToEntity(this IdentityServer4.Models.ApiScope model)
    {
        return model == null ? null : Mapper.Map<ApiScope>(model);
    }
}