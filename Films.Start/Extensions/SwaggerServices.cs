using Films.Infrastructure.Web.User.Controllers;
using Microsoft.OpenApi.Models;

namespace Films.Start.Extensions;

/// <summary>
/// Статический класс, предоставляющий метод расширения для добавления swagger в коллекцию сервисов.
/// </summary>
public static class SwaggerServices
{
    /// <summary>
    /// Добавляет swagger коллекцию сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static void AddSwaggerServices(this IServiceCollection services)
    {
        // Конфигурация Swagger для API.
        services.AddSwaggerGen(c =>
        {
            // Устанавливаем информацию о версии и названии API
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ServerSideApi", Version = "v1" });

            // Добавляем определение безопасности для Bearer токена
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            // Добавляем требование безопасности для Bearer токена
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
            
            // Получаем путь к xml файлу с документацией
            var xmlFilename = $"{typeof(UserController).Assembly.GetName().Name}.xml";
            
            // Добавляет XML комментарии для документации Swagger
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }
}