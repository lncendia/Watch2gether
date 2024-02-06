using System.Text;
using Films.Start.Exceptions;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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
        // Получение значения "Client:Audience" из конфигурации. Если значение отсутствует, генерируется исключение ConfigurationException
        var audience = configuration["Client:Audience"] ?? throw new ConfigurationException("Client:Audience");

        // Получение значения "Client:Secret" из конфигурации. Если значение отсутствует, генерируется исключение ConfigurationException
        var secret = configuration["Client:Secret"] ?? throw new ConfigurationException("Client:Secret");

        // Получение значения "Client:Issuer" из конфигурации. Если значение отсутствует, генерируется исключение ConfigurationException
        var issuer = configuration["Client:Issuer"] ?? throw new ConfigurationException("Client:Issuer");


        services.Configure<IdentityOptions>(options =>
        {
            options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
            options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
            options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
        });

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
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Укзывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = true,

                    // Установка издателя токена
                    ValidIssuer = issuer,

                    // Будет ли валидироваться потребитель токена
                    ValidateAudience = true,

                    // Установка потребителя токена
                    ValidAudience = audience,

                    // Будет ли валидироваться время существования
                    ValidateLifetime = true,

                    // Установка ключа безопасности
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),

                    // Валидация ключа безопасности
                    ValidateIssuerSigningKey = true
                };

                // Указывает, что проверка HTTPS-метаданных не требуется
                options.RequireHttpsMetadata = false;
            });

        // Добавляет службы политики авторизации в указанную коллекцию IServiceCollection.
        services.AddAuthorizationBuilder()

            // Добавляет службы политики авторизации в указанную коллекцию IServiceCollection.
            .AddPolicy("admin", policy => { policy.RequireClaim(JwtClaimTypes.Role, "admin"); });
    }
}