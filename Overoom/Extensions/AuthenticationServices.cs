using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Identity;
using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.Users.Entities;
using Overoom.Application.Abstractions.Users.Interfaces;
using Overoom.Application.Services.Users;
using Overoom.Infrastructure.ApplicationData;
using Overoom.WEB.RoomAuthentication;

namespace Overoom.Extensions;

public static class AuthenticationServices
{
    public static void AddAuthenticationServices(this IServiceCollection services)
    {
        
        services.AddTransient<IUserValidator<UserData>, UserValidator>();
        services.AddIdentity<UserData, RoleData>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.ClaimsIdentity.UserIdClaimType = ClaimTypes.Sid;
        }).AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
        
        services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();

        services.AddAuthentication()
            .AddVkontakte(options =>
            {
                options.ClientId = "51581170";
                options.ClientSecret = "IBStj9xwbmbf1j4ouzoj";
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
            options.AddPolicy("FilmRoom", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddAuthenticationSchemes(ApplicationConstants.RoomScheme);
                policy.RequireClaim("RoomId");
                policy.RequireClaim(ClaimTypes.NameIdentifier);
                policy.RequireClaim(ClaimTypes.Name);
                policy.RequireClaim(ApplicationConstants.AvatarClaimType);
                policy.RequireClaim("RoomType", RoomType.Film.ToString());
            });
            options.AddPolicy("YoutubeRoom", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddAuthenticationSchemes(ApplicationConstants.RoomScheme);
                policy.RequireClaim("RoomId");
                policy.RequireClaim(ClaimTypes.NameIdentifier);
                policy.RequireClaim(ClaimTypes.Name);
                policy.RequireClaim(ApplicationConstants.AvatarClaimType);
                policy.RequireClaim("RoomType", RoomType.Youtube.ToString());
            });
            options.AddPolicy("User", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddAuthenticationSchemes(IdentityConstants.ApplicationScheme);
                policy.RequireClaim(ClaimTypes.Name);
                policy.RequireClaim(ApplicationConstants.AvatarClaimType);
                policy.RequireClaim(ClaimTypes.NameIdentifier);
            });
            options.AddPolicy("Admin", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddAuthenticationSchemes(IdentityConstants.ApplicationScheme);
                policy.RequireClaim(ClaimTypes.Name);
                policy.RequireRole(ApplicationConstants.AdminRoleName);
                policy.RequireClaim(ApplicationConstants.AvatarClaimType);
                policy.RequireClaim(ClaimTypes.NameIdentifier);
            });
        });
    }
}