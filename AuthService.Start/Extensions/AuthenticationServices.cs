using AuthService.Application.Abstractions.Entities;
using AuthService.Infrastructure.Storage;
using AuthService.Infrastructure.Web.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Start.Extensions;

public static class AuthenticationServices
{
    private const string AllowedUserNameCharacters =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯабвгдеёжзийклмнопрстуфхцчшщьыъэюя0123456789.@-_+ ";

    public static void AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentity<UserData, RoleData>(options =>
            {
                // Разрешает применение механизма блокировки для новых пользователей.
                options.Lockout.AllowedForNewUsers = true;

                // Задает временной интервал блокировки по умолчанию в 15 минут
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

                // Устанавливает максимальное количество неудачных попыток входа перед блокировкой
                options.Lockout.MaxFailedAccessAttempts = 10;

                options.User.RequireUniqueEmail = true;
                options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
                options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
                options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
                options.ClaimsIdentity.EmailClaimType = JwtClaimTypes.Email;
                options.User.AllowedUserNameCharacters = AllowedUserNameCharacters;
            })
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();

        var issuer = configuration.GetRequiredValue<string>("JwtSettings:Issuer");
        var audience = configuration.GetSection("JwtSettings:Audience").Get<string[]>() ?? [];
        var secret = configuration.GetRequiredValue<string>("JwtSettings:Secret");
        var lifetime = configuration.GetRequiredValue<int>("JwtSettings:TokenLifeTime");
        string[] claims =
            [JwtClaimTypes.Subject, JwtClaimTypes.Name, JwtClaimTypes.Role, JwtClaimTypes.Email, JwtClaimTypes.Picture];

        services.AddScoped<IJwtService, JwtService>(sp =>
            new JwtService(issuer, audience, secret, lifetime, claims,
                sp.GetRequiredService<IUserClaimsPrincipalFactory<UserData>>()));
    }
}