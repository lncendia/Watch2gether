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
    
    /// <summary>
    /// URL клиента
    /// </summary>
    private const string ClientAngularUrl = "https://localhost:10003";

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
                ClientId = "client_react",

                // Отображаемое имя клиента (используется для ведения журнала и экрана согласия)
                ClientName = "PJMS React",

                // URL клиента
                ClientUri = ClientReactUrl,

                // Логотип клиента
                LogoUri =
                    "https://avatars.dzeninfra.ru/get-zen-logos/1597769/pubsuite_6fd98e99-a42c-4e71-8572-22dbce8ba98a_62b04a0102b70f428a0fdab4/xxh",

                // Указывает, является ли токен доступа ссылочным токеном или
                // автономным токеном JWT (по умолчанию Jwt).
                AccessTokenType = AccessTokenType.Jwt,

                // Указывает, требуется ли экран согласия (по умолчанию false)
                RequireConsent = false,

                // Время жизни токена доступа в секундах (по умолчанию 3600 секунд/1 час)
                AccessTokenLifetime = 180,

                // Время жизни токена идентификации в секундах (по умолчанию 300 секунд/5 минут)
                IdentityTokenLifetime = 180,

                // Если установлено значение false, секрет клиента не требуется
                // для запроса токенов в конечной точке токена (по умолчанию — true).
                RequireClientSecret = false,

                // Указывает разрешенные типы предоставления (допустимые комбинации
                // AuthorizationCode, Implicit, Hybrid, ResourceOwner, ClientCredentials).
                AllowedGrantTypes = GrantTypes.Code,

                // Указывает, требуется ли ключ проверки для запросов токена
                // на основе кода авторизации (по умолчанию — true).
                RequirePkce = true,

                // Определяет, передаются ли маркеры доступа через браузер для
                // этого клиента (по умолчанию false). Это может предотвратить
                // случайную утечку маркеров доступа, когда разрешено несколько типов ответов.
                AllowAccessTokensViaBrowser = true,

                // Получает или задает значение, указывающее, разрешен
                // ли [разрешить автономный доступ]. По умолчанию ложно.
                AllowOfflineAccess = true,

                // Получает или задает значение, указывающее, должны ли клиентские
                // утверждения всегда включаться в маркеры доступа или только для
                // потока учетных данных клиента. По умолчанию ложно
                AlwaysSendClientClaims = true,

                // При запросе маркера идентификатора и маркера доступа утверждения
                // пользователя всегда должны добавляться к маркеру идентификатора
                // вместо того, чтобы требовать от клиента использования конечной точки
                // userinfo. По умолчанию ложно.
                AlwaysIncludeUserClaimsInIdToken = true,

                // Получает или задает разрешенные источники CORS для клиентов JavaScript.
                AllowedCorsOrigins =
                {
                    $"{ClientReactUrl}"
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
                    "Business",
                    StandardScopes.OpenId,
                    StandardScopes.Profile,
                    StandardScopes.Email,
                    "roles"
                }
            },
            
             // Фронтэнд клиент
            new()
            {
                // Уникальный идентификатор клиента
                ClientId = "client_angular",

                // Отображаемое имя клиента (используется для ведения журнала и экрана согласия)
                ClientName = "PJMS Angular",

                // Указывает, является ли токен доступа ссылочным токеном или
                // автономным токеном JWT (по умолчанию Jwt).
                AccessTokenType = AccessTokenType.Jwt,

                // Указывает, требуется ли экран согласия (по умолчанию false)
                RequireConsent = true,

                // Время жизни токена доступа в секундах (по умолчанию 3600 секунд/1 час)
                AccessTokenLifetime = 180,

                // Время жизни токена идентификации в секундах (по умолчанию 300 секунд/5 минут)
                IdentityTokenLifetime = 180,

                // Если установлено значение false, секрет клиента не требуется
                // для запроса токенов в конечной точке токена (по умолчанию — true).
                RequireClientSecret = false,

                // Указывает разрешенные типы предоставления (допустимые комбинации
                // AuthorizationCode, Implicit, Hybrid, ResourceOwner, ClientCredentials).
                AllowedGrantTypes = GrantTypes.Code,

                // Указывает, требуется ли ключ проверки для запросов токена
                // на основе кода авторизации (по умолчанию — true).
                RequirePkce = true,

                // Определяет, передаются ли маркеры доступа через браузер для
                // этого клиента (по умолчанию false). Это может предотвратить
                // случайную утечку маркеров доступа, когда разрешено несколько типов ответов.
                AllowAccessTokensViaBrowser = true,

                // Получает или задает значение, указывающее, должны ли клиентские
                // утверждения всегда включаться в маркеры доступа или только для
                // потока учетных данных клиента. По умолчанию ложно
                AlwaysSendClientClaims = true,

                // При запросе маркера идентификатора и маркера доступа утверждения
                // пользователя всегда должны добавляться к маркеру идентификатора
                // вместо того, чтобы требовать от клиента использования конечной точки
                // userinfo. По умолчанию ложно.
                AlwaysIncludeUserClaimsInIdToken = true,

                // Получает или задает разрешенные источники CORS для клиентов JavaScript.
                AllowedCorsOrigins =
                {
                    $"{ClientAngularUrl}"
                },

                // Указывает разрешенные URI для возврата токенов или кодов авторизации
                RedirectUris =
                {
                    $"{ClientAngularUrl}/",
                    $"{ClientAngularUrl}/adminPage",
                    $"{ClientAngularUrl}/silent-refresh.html"
                },

                // Указывает разрешенные URI для перенаправления после выхода из системы.
                PostLogoutRedirectUris =
                {
                    $"{ClientAngularUrl}/",
                    $"{ClientAngularUrl}/unauthorized",
                    $"{ClientAngularUrl}/adminPage",
                    $"{ClientAngularUrl}"
                },

                // Указывает области API, которые разрешено запрашивать клиенту.
                // Если пусто, клиент не может получить доступ ни к какой области.
                AllowedScopes =
                {
                    "Business",
                    StandardScopes.OpenId,
                    StandardScopes.Profile,
                    StandardScopes.Email,
                    "roles"
                }
            },

            // Клиент с бизнес логикой
            new()
            {
                // Уникальный идентификатор клиента
                ClientId = "client_business",

                // Секреты клиента — актуальны только для потоков, которым требуется секрет
                ClientSecrets = { new Secret("client_business".ToSha256()) },

                // Указывает разрешенные типы предоставления (допустимые комбинации
                // AuthorizationCode, Implicit, Hybrid, ResourceOwner, ClientCredentials).
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // Получает или задает разрешенные источники CORS для клиентов JavaScript.
                AllowedCorsOrigins = { "https://localhost:7001" },

                // Указывает области API, которые разрешено запрашивать клиенту.
                // Если пусто, клиент не может получить доступ ни к какой области.
                AllowedScopes =
                {
                    "WeatherForecast"
                },

                // Получает или задает значение, указывающее, разрешен
                // ли [разрешить автономный доступ]. По умолчанию ложно.
                AllowOfflineAccess = true,

                // Получает или задает значение, указывающее, следует ли обновлять
                // маркер доступа (и его утверждения) при запросе
                // маркера обновления. По умолчанию ложно.
                UpdateAccessTokenClaimsOnRefresh = true
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
            new("BusinessAPI", "Business API Application", new[] { JwtClaimTypes.Role })
            {
                Scopes = { "Business" },
            },
            new("WeatherForecastAPI", "Weather forecast API Application")
            {
                Scopes = { "WeatherForecast" }
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
            new("WeatherForecast", "Weather forecast"),
            new("Business", "Business"),
        };
    }
}