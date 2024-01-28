using System.Security.Claims;
using Films.Start.Exceptions;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Identity;
using Films.Start.Application.Abstractions;
using Films.Start.Application.Abstractions.Authentication.Entities;
using Films.Start.Application.Abstractions.Authentication.Interfaces;
using Films.Start.Application.Services.Authentication;
using Films.Start.Application.Services.Common;
using Films.Start.Infrastructure.ApplicationData;
using Films.Start.WEB.RoomAuthentication;

namespace Films.Start.Extensions;

public static class AuthenticationServices
{
    public static void AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        
        var vkOauth = new
        {
            Client = configuration.GetValue<string>("OAuth:Vkontakte:Client") ??
                     throw new ConfigurationException("OAuth:Vkontakte:Client"),
            Secret = configuration.GetValue<string>("OAuth:Vkontakte:Secret") ??
                     throw new ConfigurationException("OAuth:Vkontakte:Secret")
        };

        var yandexOauth = new
        {
            Client = configuration.GetValue<string>("OAuth:Yandex:Client") ??
                     throw new ConfigurationException("OAuth:Yandex:Client"),
            Secret = configuration.GetValue<string>("OAuth:Yandex:Secret") ??
                     throw new ConfigurationException("OAuth:Yandex:Secret")
        };
        
        services.AddTransient<IUserValidator<UserData>, UserValidator>();
        services.AddTransient<IPasswordValidator<UserData>, PasswordValidator>();
        services.AddIdentity<UserData, RoleData>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.ClaimsIdentity.UserIdClaimType = ClaimTypes.Sid;
        }).AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
        
        services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();

        services.AddAuthentication()
            .AddVkontakte(options =>
            {
                options.ClientId = vkOauth.Client;
                options.ClientSecret = vkOauth.Secret;
                options.Scope.Add("email");
                options.Scope.Add("photos");
            }).AddYandex(options =>
            {
                options.ClientId = yandexOauth.Client;
                options.ClientSecret = yandexOauth.Secret;
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
                policy.RequireClaim("RoomType", RoomType.Film.ToString());
            });
            options.AddPolicy("YoutubeRoom", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddAuthenticationSchemes(ApplicationConstants.RoomScheme);
                policy.RequireClaim("RoomId");
                policy.RequireClaim(ClaimTypes.NameIdentifier);
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