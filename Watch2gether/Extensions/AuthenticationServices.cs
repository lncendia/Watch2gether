using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Identity;
using Watch2gether.Application.Abstractions;
using Watch2gether.Application.Abstractions.Entities.Role;
using Watch2gether.Application.Abstractions.Entities.User;
using Watch2gether.Application.Abstractions.Interfaces.Users;
using Watch2gether.Application.Services.Services;
using Watch2gether.Application.Services.Services.Users;
using Watch2gether.Infrastructure.ApplicationData;

namespace Watch2gether.Extensions;

public static class AuthenticationServices
{
    public static void AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddIdentity<UserData, RoleData>(options => { options.SignIn.RequireConfirmedAccount = true; })
            .AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
        
        services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
        
        services.AddAuthentication()
            .AddVkontakte(options =>
            {
                options.ClientId = "7482215";
                options.ClientSecret = "Cq44grlZooUDOvLsJJEh";
                options.Scope.Add("email");
                options.Scope.Add("photos");
            }).AddYandex(options =>
            {
                options.ClientId = "862fc97020224f29829e9bb333e85091";
                options.ClientSecret = "ccd13995b6f8410e965f357b029b36c5";
                options.Scope.Add("login:avatar");
                options.ClaimActions.Add(new JsonKeyClaimAction("urn:yandex:user:avatar", ClaimValueTypes.String,
                    "default_avatar_id"));
            }).AddCookie(ApplicationConstants.RoomScheme);

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RoomTemporary", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddAuthenticationSchemes(ApplicationConstants.RoomScheme);
                policy.RequireClaim("RoomId");
                policy.RequireClaim(ClaimTypes.NameIdentifier);
                policy.RequireClaim(ClaimTypes.Name);
                policy.RequireClaim(ClaimTypes.Thumbprint);
            });
            options.AddPolicy("Identity.Application", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddAuthenticationSchemes(IdentityConstants.ApplicationScheme);
                policy.RequireClaim(ClaimTypes.Name);
            });
            options.AddPolicy("FilmDownloader", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddAuthenticationSchemes(IdentityConstants.ApplicationScheme);
                policy.RequireClaim(ClaimTypes.Name);
                policy.RequireRole(ApplicationConstants.AdminRoleName);
            });
        });

    }
}