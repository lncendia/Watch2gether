using Films.Infrastructure.Web.Films.Controllers;
using Microsoft.OpenApi.Models;

namespace Films.Start.Extensions;

/// <summary>
/// Класс для настройки Swagger в приложении.
/// </summary>
public static class SwaggerServices
{
    /// <summary>
    /// Добавляет настройки Swagger в коллекцию сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static void AddSwaggerServices(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{typeof(FilmsController).Assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            // Включение human-friendly описаний для операций, параметров и схем на
            // основе файлов комментариев XML
            options.IncludeXmlComments(xmlPath);

            options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            // Определите схему OAuth2 для Swagger
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri("https://localhost:10001/connect/authorize"),
                        TokenUrl = new Uri("https://localhost:10001/connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { "Films", "Доступ к библиотеке фильмов" }
                        }
                    }
                }
            });

            // Используйте схему OAuth2 для всех операций в Swagger
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        },
                        Scheme = "oauth2",
                        Name = "oauth2",
                        In = ParameterLocation.Header
                    },
                    new[] { "Films" }
                }
            });
        });
    }
}