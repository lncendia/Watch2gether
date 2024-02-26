using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace PJMS.AuthService.Extensions;

/// <summary>
/// Класс для настройки Swagger в приложении.
/// </summary>
public static class Swagger
{
    private const string SwaggerConfig = "/swagger/v1/swagger.json";
    private const string SwaggerUrl = "api/manual";
    private const string AppTitle = "Search Dialogue API ";
    private const string AppVersion = "1.0.0";

    /// <summary>
    /// Добавляет настройки Swagger в коллекцию сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            //Определите одну или несколько документов, которые
            //будут созданы генератором Swagger
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = AppTitle,
                Version = AppVersion,
                Description = "Search Dialogue API module API documentation." +
                              " This API based on ASP.NET Core 5.",

                Contact = new OpenApiContact
                {
                    Name = "Sergei Zakharov",
                    Email = "sergeizahargood@gmail.com",
                    Url = new Uri("https://www.instagram.com/sergeizahargood_gmail_com/")
                },
                License = new OpenApiLicense
                {
                    Name = "MY License",
                    Url = new Uri("https://www.instagram.com/")
                }
            });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            // Включение human-friendly описаний для операций, параметров и схем на
            // основе файлов комментариев XML
            options.IncludeXmlComments(xmlPath);

            //Слияние действий, которые имеют противоречивые HTTP-методы
            //и пути (должны быть уникальными для Swagger 2.0)
            //options.ResolveConflictingActions(x => x.First());

            //Получает подраздел конфигурации с указанным ключом.
            //var url = configuration.GetSection("IdentityServer").GetValue<string>("Url");

            //Добавьте одно или несколько «SecurityDefinittions», описывающие,
            //как защищена ваша API, к генерирующему Swagger
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                //ОБЯЗАТЕЛЬНЫЙ. Расположение ключа API. Допустимые значения
                //- это «query», «header» или «Cookie».
                In = ParameterLocation.Header,

                //ОБЯЗАТЕЛЬНЫЙ. Название схемы авторизации HTTP будет
                //использоваться в заголовке авторизации, как определено в RFC7235.
                Scheme = "bearer",

                //ОБЯЗАТЕЛЬНЫЙ. Имя заголовка, запроса или параметра cookie для использования
                Name = "Authorization",

                //Краткое описание для схемы безопасности. Синтаксис
                //CommonMark (общедоступная метка) может использоваться
                //для широкого представления текста.
                Description = "Authorization token",

                //ОБЯЗАТЕЛЬНЫЙ. Тип схемы безопасности. Допустимые значения
                //- «Apikey», «HTTP», «OAUTH2», «OpenIDConnect».
                Type = SecuritySchemeType.OAuth2,

                //ОБЯЗАТЕЛЬНЫЙ. Объект, содержащий информацию о конфигурации
                //для поддерживаемых типов потоков.
                Flows = new OpenApiOAuthFlows
                {
                    //Конфигурация для пароля владельца ресурсов OAUTH.
                    Password = new OpenApiOAuthFlow
                    {
                        //ОБЯЗАТЕЛЬНЫЙ. URL-адрес токена будет использоваться для этого
                        //потока. Относится к паролю, ClientCredentials и
                        //ArmizationCode OauthFlow.
                        TokenUrl = new Uri("https://localhost:6001/connect/token"),

                        //ОБЯЗАТЕЛЬНЫЙ. Карта между именем объема и краткое описание для него.
                        Scopes = new Dictionary<string, string>
                        {
                            //{ "api1", "Default scope" }
                        }
                    }
                }
            });

            //Добавляет требование глобальной безопасности
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        //ссылка на объект
                        Reference = new OpenApiReference
                        {
                            //тип ссылки
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        //ОБЯЗАТЕЛЬНЫЙ. Расположение ключа API. Допустимые значения
                        //- это «query», «header» или «Cookie».
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });
    }

    /// <summary>
    /// Свойства для swagger UI
    /// </summary>
    /// <param name="settings"></param>
    public static void SwaggerSettings(SwaggerUIOptions settings)
    {
        //Добавляет конечные точки Swagger JSON. Может быть полностью
        //квалифицирован или относительно UI интерфейса страницы  
        settings.SwaggerEndpoint(SwaggerConfig, $"{AppTitle} v.{AppVersion}");

        //Получает или устанавливает заголовок для страницы Swagger-UI
        settings.DocumentTitle = "Search Dialogue API";

        //Получает или задает префикс маршрута для доступа к Swagger-ui
        settings.RoutePrefix = "docs";

        //Управляет настройкой расширения по умолчанию для операций и тегов.
        //Это может быть «List» (расширяет только теги), «Full» (расширяет теги и операции)
        //или «None» (ничего не расширяется)
        settings.DocExpansion(DocExpansion.List);

        //clientId по умолчанию
        settings.OAuthClientId("client_business");

        //Scope разделитель для прохождения областей, закодированных перед вызовом,
        //значение по умолчанию - это пространство (кодированное значение% 20)
        settings.OAuthScopeSeparator(" ");

        //Default clientSecret
        settings.OAuthClientSecret("client_secret_swagger");

        //Управляет отображением продолжительности запроса (в миллисекундах)
        //для запросов TRY-IT-OUT
        //settings.DisplayRequestDuration();

        //Имя приложения, отображаемое в авторизации
        //settings.OAuthAppName("Microservice API module API documentation");

        //Глубина расширения по умолчанию для модели на раздел «Модель»
        //settings.DefaultModelExpandDepth(0);

        //Управляет, как модель отображается, когда API сначала отображается.
        //(Пользователь всегда может переключать рендеринг для заданной модели,
        //нажав ссылки «Model» и «Example Value».)
        //settings.DefaultModelRendering(ModelRendering.Model);

        //Глубина расширения по умолчанию для моделей (установлена на 1 полностью
        //скрыть модели)
        //settings.DefaultModelsExpandDepth(0);
    }
}