using AutoMapper;
using PJMS.AuthService.Data.IdentityServer.Entities;

namespace PJMS.AuthService.Data.IdentityServer.Mappers;

/// <summary>
/// Extension methods to map to/from entity/model for API resources.
/// </summary>
public static class ApiResourceMappers
{
    static ApiResourceMappers()
    {
        Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ApiResourceMapperProfile>())
            .CreateMapper();
    }

    private static IMapper Mapper { get; }

    /// <summary>
    /// Maps an entity to a model.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    public static IdentityServer4.Models.ApiResource ToModel(this ApiResource entity)
    {
        return entity == null ? null : Mapper.Map<IdentityServer4.Models.ApiResource>(entity);
    }

    /// <summary>
    /// Maps a model to an entity.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    public static ApiResource ToEntity(this IdentityServer4.Models.ApiResource model)
    {
        return model == null ? null : Mapper.Map<ApiResource>(model);
    }
}