using IdentityModel;
using IdentityServer4.Models;
using static IdentityServer4.IdentityServerConstants;

namespace PJMS.AuthService.Data.IdentityServer.HostExtensions;

/// <summary>
/// Класс устанавливает конфигурацию IdentityServer
/// </summary>
public static class IdentityServerConfiguration
{
    /// <summary>
    /// URL клиента
    /// </summary>
    private const string ClientReactUrl = "https://localhost:5173";

    private static readonly string[] SwaggerUrls =
    [
        "https://localhost:7131"
    ];

    /// <summary>
    /// Добавляет список клиентов (настройка подключенных клиентов)
    /// </summary>
    /// <returns>Список клиентов</returns>
    public static IEnumerable<Client> GetClients() =>
        new List<Client>
        {

            // Фронтэнд клиент
            new()
            {
                // Уникальный идентификатор клиента
                ClientId = "overoom_react",

                // Отображаемое имя клиента (используется для ведения журнала и экрана согласия)
                ClientName = "Overoom React",

                // URL клиента
                ClientUri = ClientReactUrl,

                // Логотип клиента
                LogoUri =
                    "https://avatars.dzeninfra.ru/get-zen-logos/1597769/pubsuite_6fd98e99-a42c-4e71-8572-22dbce8ba98a_62b04a0102b70f428a0fdab4/xxh",

                // Если установлено значение false, секрет клиента не требуется
                // для запроса токенов в конечной точке токена (по умолчанию — true).
                RequireClientSecret = false,

                // Указывает разрешенные типы предоставления (допустимые комбинации
                // AuthorizationCode, Implicit, Hybrid, ResourceOwner, ClientCredentials).
                AllowedGrantTypes = GrantTypes.Code,

                // Определяет, передаются ли маркеры доступа через браузер для
                // этого клиента (по умолчанию false). Это может предотвратить
                // случайную утечку маркеров доступа, когда разрешено несколько типов ответов.
                AllowAccessTokensViaBrowser = true,

                // При запросе маркера идентификатора и маркера доступа утверждения
                // пользователя всегда должны добавляться к маркеру идентификатора
                // вместо того, чтобы требовать от клиента использования конечной точки
                // userinfo. По умолчанию ложно.
                AlwaysIncludeUserClaimsInIdToken = true,

                // Получает или задает разрешенные источники CORS для клиентов JavaScript.
                AllowedCorsOrigins =
                {
                    ClientReactUrl
                },

                // Указывает разрешенные URI для возврата токенов или кодов авторизации
                RedirectUris =
                {
                    $"{ClientReactUrl}/signin-oidc",
                    $"{ClientReactUrl}/signin-silent-oidc"
                },
                // Указывает разрешенные URI для перенаправления после выхода из системы.
                PostLogoutRedirectUris =
                {
                    $"{ClientReactUrl}/signout-oidc"
                },

                // Указывает области API, которые разрешено запрашивать клиенту.
                // Если пусто, клиент не может получить доступ ни к какой области.
                AllowedScopes =
                {
                    "Films",
                    "Rooms",
                    StandardScopes.OpenId,
                    StandardScopes.Profile,
                    "roles"
                }
            },
            // Swagger клиент
            new()
            {
                // Уникальный идентификатор клиента
                ClientId = "swagger",

                // Отображаемое имя клиента (используется для ведения журнала и экрана согласия)
                ClientName = "Swagger",
                
                // Если установлено значение false, секрет клиента не требуется
                // для запроса токенов в конечной точке токена (по умолчанию — true).
                RequireClientSecret = false,

                // Указывает разрешенные типы предоставления (допустимые комбинации
                // AuthorizationCode, Implicit, Hybrid, ResourceOwner, ClientCredentials).
                AllowedGrantTypes = GrantTypes.Code,

                // Указывает, что пользователю нужно дать согласие.
                RequireConsent = true,
                
                // Определяет, передаются ли маркеры доступа через браузер для
                // этого клиента (по умолчанию false). Это может предотвратить
                // случайную утечку маркеров доступа, когда разрешено несколько типов ответов.
                AllowAccessTokensViaBrowser = true,

                // При запросе маркера идентификатора и маркера доступа утверждения
                // пользователя всегда должны добавляться к маркеру идентификатора
                // вместо того, чтобы требовать от клиента использования конечной точки
                // userinfo. По умолчанию ложно.
                AlwaysIncludeUserClaimsInIdToken = true,

                // Получает или задает разрешенные источники CORS для клиентов JavaScript.
                AllowedCorsOrigins = SwaggerUrls,

                // Указывает разрешенные URI для возврата токенов или кодов авторизации
                RedirectUris = SwaggerUrls.Select(url=>$"{url}/swagger/oauth2-redirect.html").ToArray(),

                // Указывает области API, которые разрешено запрашивать клиенту.
                // Если пусто, клиент не может получить доступ ни к какой области.
                AllowedScopes =
                {
                    "Films"
                }
            }
        };

    /// <summary>
    /// Получение (указание) API ресурсов которые могут взаимодействовать c 
    /// сервером авторизации. Название ресурсов указываются в GetClients() =>
    /// new Client => AllowedScopes
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<ApiResource> GetApiResources()
    {
        var list = new List<ApiResource>
        {
            new("Films", "Access to films library", new[] { JwtClaimTypes.Role })
            {
                Scopes = { "Films" }
            },
            new("Rooms_1", "Access to rooms server 1")
            {
                Scopes = { "Rooms" }
            }
        };
        return list;
    }

    /// <summary>
    /// Запрос утверждений (Scopes) о пользователе.
    /// Ссылка на документацию:
    /// "https://identityserver4.readthedocs.io/en/latest/quickstarts/2_interactive_aspnetcore.html#:~:text=not%20registered%20yet.-,Adding%20support%20for%20OpenID%20Connect%20Identity%20Scopes,-Similar%20to%20OAuth"
    /// </summary>
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
        {
            // сообщает провайдеру о необходимости возврата утверждения sub (идентификатора
            // субъекта) в токене идентификации.
            new IdentityResources.OpenId(),

            // представляет отображаемое имя, утверждение веб-сайта и тд.
            new IdentityResources.Profile(),

            // представляет адрес электронной почты
            new IdentityResources.Email(),

            // представляет роли
            new("roles", new[] { JwtClaimTypes.Role })
        };
    }

    /// <summary>
    /// IdentityServer4 version 4.x.x changes
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>
        {
            new("Films", "Films library"),
            new("Rooms", "Rooms service")
        };
    }
}