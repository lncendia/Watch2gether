using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Films.Start.Extensions;

/// <summary>
/// Статический класс, предоставляющий метод расширения для добавления авторизации по JWT в коллекцию сервисов.
/// </summary>
public static class JwtAuthorization
{
    /// <summary>
    /// Добавляет авторизацию по JWT в коллекцию сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    public static void AddJwtAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        // Получение значения "Client:Authority" из конфигурации.
        var authority = configuration.GetRequiredValue<string>("Authorization:Authority");

        // Получение значения "Client:Audience" из конфигурации.
        var audience = configuration.GetRequiredValue<string>("Authorization:Audience");

        // Регистрация аутентификации с настройками по умолчанию
        services.AddAuthentication(options =>
            {
                // Установка схемы аутентификации по умолчанию
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                // Установка схемы для изменения по умолчанию
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Конфигурация параметров JwtBearer
            .AddJwtBearer(options =>
            {
                // Адрес IdentityServer
                options.Authority = authority;

                // Идентификатор ресурса API, который вы задали в IdentityServer
                options.Audience = audience;

                // Валидация токена
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true
                };
            });
        
        // Добавляет службы политики авторизации в указанную коллекцию IServiceCollection.
        services.AddAuthorizationBuilder()

            // Добавляет службы политики авторизации в указанную коллекцию IServiceCollection.
            .AddPolicy("admin", policy => { policy.RequireClaim(ClaimTypes.Role, "admin"); });
    }
}