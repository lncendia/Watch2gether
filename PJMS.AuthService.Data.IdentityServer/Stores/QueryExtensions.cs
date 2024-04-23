using Microsoft.EntityFrameworkCore;
using PJMS.AuthService.Data.IdentityServer.Entities;

namespace PJMS.AuthService.Data.IdentityServer.Stores;

public static class QueryExtensions
{
    public static IQueryable<Client> LoadDependencies(this IQueryable<Client> queryable)
    {
        return queryable
            .Include(x => x.AllowedGrantTypes)
            .Include(x => x.RedirectUris)
            .Include(x => x.PostLogoutRedirectUris)
            .Include(x => x.AllowedScopes)
            .Include(x => x.ClientSecrets)
            .Include(x => x.Claims)
            .Include(x => x.IdentityProviderRestrictions)
            .Include(x => x.AllowedCorsOrigins)
            .Include(x => x.Properties);
    }

    public static IQueryable<ApiResource> LoadDependencies(this IQueryable<ApiResource> queryable)
    {
        return queryable
            .Include(x => x.Secrets)
            .Include(x => x.Scopes)
            .Include(x => x.UserClaims)
            .Include(x => x.Properties);
    }

    public static IQueryable<IdentityResource> LoadDependencies(this IQueryable<IdentityResource> queryable)
    {
        return queryable
            .Include(x => x.UserClaims)
            .Include(x => x.Properties);
    }
    
    public static IQueryable<ApiScope> LoadDependencies(this IQueryable<ApiScope> queryable)
    {
        return queryable
            .Include(x => x.UserClaims)
            .Include(x => x.Properties);
    }
}