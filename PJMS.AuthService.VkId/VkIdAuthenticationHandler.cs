using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace PJMS.AuthService.VkId;

/// <summary> 
/// Обработчик аутентификации ВКонтакте для OAuth. 
/// </summary>
/// <param name="options">Параметры аутентификации ВКонтакте.</param> 
/// <param name="logger">Фабрика логгеров.</param> 
/// <param name="encoder">Кодировщик URL.</param> 
public partial class VkIdAuthenticationHandler(
    IOptionsMonitor<VkIdAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : OAuthHandler<VkIdAuthenticationOptions>(options, logger, encoder)
{
    /// <summary> 
    /// Переопределение метода для создания URL вызова аутентификации ВКонтакте. 
    /// </summary> 
    /// <param name="properties">Свойства аутентификации.</param> 
    /// <param name="redirectUri">URL перенаправления после аутентификации.</param> 
    /// <returns>URL вызова аутентификации.</returns> 
    protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
    {
        // Коллекция параметров url стоки
        var parameters = new Dictionary<string, string>
        {
            {
                // Добавление параметра "app_id" с значением ClientId из опций аутентификации 
                "app_id", Options.ClientId
            },
            {
                // Добавление параметра "response_type" со значением "code" 
                "response_type", "code"
            },
            {
                // Добавление параметра "redirect_uri" со значением переданного redirectUri 
                "redirect_uri", redirectUri
            },
            {
                // Добавление параметра "uuid" со случайно сгенерированным значением 
                "uuid", new Random().Next(1, int.MaxValue).ToString()
            }
        };

        // Добавление параметра "redirect_state" с зашифрованными данными свойств аутентификации 
        parameters["redirect_state"] = Options.StateDataFormat.Protect(properties);

        // Построение URL вызова аутентификации с добавленными параметрами
        return QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, parameters!);
    }

    /// <summary> 
    /// Обменивает код авторизации на токен авторизации у удаленного провайдера. 
    /// </summary> 
    /// <param name="context">Контекст обмена кода авторизации.</param> 
    /// <returns>Ответ <see cref="OAuthTokenResponse"/>.</returns>
    protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(OAuthCodeExchangeContext context)
    {
        // Получаем параметр uuid
        if (!context.Properties.Items.TryGetValue("uuid", out var uuid))
        {
            // Если значение uuid не найдено в свойствах аутентификации, возвращается ошибка 
            return OAuthTokenResponse.Failed(new AuthenticationFailureException("Can't find uuid"));
        }

        // Параметры строки url
        var tokenRequestParameters = new Dictionary<string, string>
        {
            // Добавление параметра "access_token" с значением ClientSecret из опций аутентификации 
            { "access_token", Options.ClientSecret },
            // Добавление параметра "token" с значением кода авторизации из контекста 
            { "token", context.Code },
            // Добавление параметра "uuid" с полученным значением uuid 
            { "uuid", uuid! },
            // Добавление параметра "v" с версией API из опций аутентификации 
            { "v", Options.ApiVersion }
        };

        // Создание контента запроса с параметрами токена 
        var requestContent = new FormUrlEncodedContent(tokenRequestParameters);

        // Создание HTTP-запроса POST на эндпоинт токена 
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);

        // Установка заголовка Accept для получения JSON-ответа 
        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // Установка контента запроса 
        requestMessage.Content = requestContent;

        // Установка версии запроса 
        requestMessage.Version = Backchannel.DefaultRequestVersion;

        // Отправка асинхронного запроса на удаленный провайдер 
        var response = await Backchannel.SendAsync(requestMessage, Context.RequestAborted);

        // Получение тела ответа в виде строки 
        var body = await response.Content.ReadAsStringAsync(Context.RequestAborted);

        // Возврат значения
        return response.IsSuccessStatusCode switch
        {
            // Если запрос успешен, парсится тело ответа в объект OAuthTokenResponse 
            true => body.GetOAuthTokenResponse(),

            // Если запрос неуспешен, возвращается ошибка 
            false => OAuthTokenResponse.Failed(new AuthenticationFailureException(
                $"OAuth token endpoint failure: Status: {response.StatusCode};Headers: {response.Headers};Body: {body};"))
        };
    }

    /// <summary> 
    /// Обработка асинхронного удаленного аутентификационного запроса. 
    /// </summary> 
    /// <returns>Результат обработки аутентификационного запроса.</returns> 
    protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
    {
        // Получение параметров запроса 
        var query = Request.Query;

        // Извлечение значения параметра "state" из запроса 
        var state = query["state"];

        // Расшифровка данных состояния аутентификации 
        var properties = Options.StateDataFormat.Unprotect(state);

        // Проверка наличия и валидности свойств аутентификации 
        if (properties == null)
        {
            // Если данных нет - возвращаем ошибку
            return HandleRequestResult.Fail("The oauth state was missing or invalid.");
        }

        // Проверка на соответствие идентификатора корреляции 
        if (!ValidateCorrelationId(properties))
        {
            // Если данные не прошли проверку - возвращаем ошибку
            return HandleRequestResult.Fail("Correlation failed.", properties);
        }

        // Извлечение значения параметра "payload" из запроса 
        var payloadStr = query["payload"];

        // Проверка наличия значения параметра "payload" 
        if (StringValues.IsNullOrEmpty(payloadStr))
        {
            // Если данных нет - возвращаем ошибку
            return HandleRequestResult.Fail("Payload was not found.");
        }

        // Разбор JSON-строки в объект JsonDocument 
        using var payload = JsonDocument.Parse(payloadStr!);

        // Извлечение значения "uuid" из JsonDocument 
        var uuid = payload.RootElement.GetString("uuid");

        // Проверка наличия значения "uuid" 
        if (string.IsNullOrEmpty(uuid))
        {
            // Если данных нет - возвращаем ошибку
            return HandleRequestResult.Fail("UUID was not found.", properties);
        }

        // Добавление значения "uuid" в свойства аутентификации 
        properties.Items["uuid"] = uuid;

        // Извлечение значения "token" из JsonDocument 
        var code = payload.RootElement.GetString("token");

        // Проверка наличия значения "token" 
        if (string.IsNullOrEmpty(code))
        {
            // Если данных нет - возвращаем ошибку
            return HandleRequestResult.Fail("Code was not found.", properties);
        }

        // Создание контекста обмена кода авторизации 
        var codeExchangeContext =
            new OAuthCodeExchangeContext(properties, code, BuildRedirectUri(Options.CallbackPath));

        // Обмен кода авторизации на токены 
        using var tokens = await ExchangeCodeAsync(codeExchangeContext);

        // Проверка наличия ошибки при обмене кода на токены 
        if (tokens.Error != null)
        {
            // Если данных нет - возвращаем ошибку
            return HandleRequestResult.Fail(tokens.Error, properties);
        }

        // Проверка наличия токена 
        if (string.IsNullOrEmpty(tokens.AccessToken))
        {
            // Если данных нет - возвращаем ошибку
            return HandleRequestResult.Fail("Failed to retrieve access token.", properties);
        }

        // Создание идентификации субъекта аутентификации 
        var identity = new ClaimsIdentity(ClaimsIssuer);

        // Сохранение токенов аутентификации, если опция SaveTokens установлена 
        if (Options.SaveTokens)
        {
            // Создание списка токенов аутентификации 
            var authTokens = new List<AuthenticationToken>
                { new() { Name = "access_token", Value = tokens.AccessToken } };

            // Проверка наличия значения RefreshToken
            if (!string.IsNullOrEmpty(tokens.RefreshToken))
            {
                // Добавление в список токенов 
                authTokens.Add(new AuthenticationToken { Name = "refresh_token", Value = tokens.RefreshToken });
            }

            // Проверка наличия значения TokenType и добавление его в список токенов 
            if (!string.IsNullOrEmpty(tokens.TokenType))
            {
                // Добавление в список токенов 
                authTokens.Add(new AuthenticationToken { Name = "token_type", Value = tokens.TokenType });
            }

            // Проверка наличия значения ExpiresIn и его преобразование в число 
            if (!string.IsNullOrEmpty(tokens.ExpiresIn))
            {
                // Если ExpiresIn присутствует - преобразовуем в int
                if (int.TryParse(tokens.ExpiresIn, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
                {
                    // Вычисление времени истечения срока действия токена 
                    var expiresAt = TimeProvider.GetUtcNow() + TimeSpan.FromSeconds(value);

                    // Добавление в список токенов 
                    authTokens.Add(new AuthenticationToken
                    {
                        Name = "expires_at",
                        Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
                    });
                }
            }

            // Сохранение токенов в свойствах аутентификации 
            properties.StoreTokens(authTokens);
        }

        // Создание билета аутентификации 
        var ticket = await CreateTicketAsync(identity, properties, tokens);

        // Возврат успешного значения с билетом
        return HandleRequestResult.Success(ticket);
    }

    /// <summary> 
    /// Асинхронное создание билета аутентификации. 
    /// </summary> 
    /// <param name="identity">Идентификация субъекта аутентификации.</param> 
    /// <param name="properties">Свойства аутентификации.</param> 
    /// <param name="tokens">Ответ токена аутентификации.</param> 
    /// <returns>Билет аутентификации.</returns> 
    protected override async Task<AuthenticationTicket> CreateTicketAsync(
        ClaimsIdentity identity,
        AuthenticationProperties properties,
        OAuthTokenResponse tokens)
    {
        // Создание словаря параметров для запроса информации о пользователе 
        var parameters = new Dictionary<string, string?>
        {
            // Параметр "access_token" с токеном доступа 
            ["access_token"] = tokens.AccessToken,

            // Параметр "v" с версией API из опций аутентификации или значением по умолчанию 
            ["v"] = !string.IsNullOrEmpty(Options.ApiVersion)
                ? Options.ApiVersion
                : VkIdAuthenticationDefaults.ApiVersion
        };

        // Добавление параметров к URL эндпоинта получения информации о пользователе 
        var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, parameters);

        // Добавление параметра "fields" с указанными полями, если они заданы 
        if (Options.Fields.Count != 0)
        {
            address = QueryHelpers.AddQueryString(address, "fields", string.Join(',', Options.Fields));
        }

        // Отправка асинхронного GET-запроса на удаленный эндпоинт 
        using var response = await Backchannel.GetAsync(address, Context.RequestAborted);

        // Проверка успешного статуса ответа 
        if (!response.IsSuccessStatusCode)
        {
            // Логирование ошибки получения профиля пользователя 
            await Log.UserProfileErrorAsync(Logger, response, Context.RequestAborted);

            // Вызываем исключение
            throw new HttpRequestException("An error occurred while retrieving the user profile.");
        }

        // Разбор JSON-ответа в объект JsonDocument 
        using var container = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));
        using var enumerator = container.RootElement.GetProperty("response").EnumerateArray();

        // Получение первого элемента из массива ответа 
        var payload = enumerator.First();

        // Создание принципала с идентификацией субъекта аутентификации 
        var principal = new ClaimsPrincipal(identity);

        // Создание контекста для создания билета аутентификации 
        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel,
            tokens, payload);

        // Выполнение действий с утверждениями 
        context.RunClaimActions();

        // Повторное выполнение действий с утверждениями для получения email из ответа на токены 
        context.RunClaimActions(tokens.Response!.RootElement);

        // Вызов события CreatingTicket 
        await Events.CreatingTicket(context);

        // Возвращение билета аутентификации 
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    /// <summary> 
    /// Класс, содержащий методы для логирования ошибок получения профиля пользователя. 
    /// </summary> 
    private static partial class Log
    {
        /// <summary> 
        /// Асинхронное логирование ошибки получения профиля пользователя. 
        /// </summary> 
        /// <param name="logger">Логгер.</param> 
        /// <param name="response">Ответ на запрос.</param> 
        /// <param name="cancellationToken">Токен отмены операции.</param> 
        internal static async Task UserProfileErrorAsync(ILogger logger, HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            UserProfileError(logger, response.StatusCode, response.Headers.ToString(), await response.Content.ReadAsStringAsync(cancellationToken));
        }

        /// <summary> 
        /// Логирование ошибки получения профиля пользователя. 
        /// </summary> 
        /// <param name="logger">Логгер.</param> 
        /// <param name="status">Статус ответа.</param> 
        /// <param name="headers">Заголовки ответа.</param> 
        /// <param name="body">Тело ответа.</param> 
        [LoggerMessage(1, LogLevel.Error,
            "An error occurred while retrieving the user profile: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        private static partial void UserProfileError(ILogger logger, System.Net.HttpStatusCode status, string headers, string body);
    }
}