using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PJMS.AuthService.VkId;

namespace PJMS.AuthService.Extensions;

// / <summary>
// / Статический класс, представляющий методы для добавления OAuth аутентификации.
// / </summary>
public static class Oauth
{
    // / <summary>
    // / Добавляет OAuth аутентификацию.
    // / </summary>
    // / <param name="services">Коллекция сервисов.</param>
    // / <param name="configuration">Конфигурация приложения.</param>
    public static void AddOauth(this IServiceCollection services, IConfiguration configuration)
    {
        // Создаем объект vkOauth, содержащий информацию о настройках OAuth для ВКонтакте.
        var vkOauth = new
        {
            Client = configuration.GetRequiredValue<string>("OAuth:VkId:Client"),
            Secret = configuration.GetRequiredValue<string>("OAuth:VkId:Secret")
        };

        // Создаем объект yandexOauth, содержащий информацию о настройках OAuth для Яндекса.
        var yandexOauth = new
        {
            Client = configuration.GetRequiredValue<string>("OAuth:Yandex:Client"),
            Secret = configuration.GetRequiredValue<string>("OAuth:Yandex:Secret")
        };

        // Создаем объект githubOauth, содержащий информацию о настройках OAuth для GitHub.
        var githubOauth = new
        {
            Client = configuration.GetRequiredValue<string>("OAuth:GitHub:Client"),
            Secret = configuration.GetRequiredValue<string>("OAuth:GitHub:Secret")
        };

        // Создаем объект googleOauth, содержащий информацию о настройках OAuth для Google.
        var googleOauth = new
        {
            Client = configuration.GetRequiredValue<string>("OAuth:Google:Client"),
            Secret = configuration.GetRequiredValue<string>("OAuth:Google:Secret")
        };

        // Создаем объект microsoftOauth, содержащий информацию о настройках OAuth для Microsoft.
        var microsoftOauth = new
        {
            Client = configuration.GetRequiredValue<string>("OAuth:Microsoft:Client"),
            Secret = configuration.GetRequiredValue<string>("OAuth:Microsoft:Secret")
        };

        // Создаем объект xOauth, содержащий информацию о настройках OAuth для X.
        var xOauth = new
        {
            Client = configuration.GetRequiredValue<string>("OAuth:X:Client"),
            Secret = configuration.GetRequiredValue<string>("OAuth:X:Secret")
        };

        // Добавляет аутентификацию с помощью внешних сервисов
        services.AddAuthentication()

            // Добавляет аутентификацию через Yandex
            .AddYandex(options =>
            {
                // Id oauth приложения
                options.ClientId = yandexOauth.Client;

                // Secret oauth приложения
                options.ClientSecret = yandexOauth.Secret;

                options.Scope.Add("login:avatar");

                // Добавляем маппинг для клайма photo:link
                options.ClaimActions.MapCustomJson("photo:link", user =>
                {
                    // Получаем значение с ссылкой на фото профиля
                    var value = user.TryGetProperty("default_avatar_id", out var picture);

                    // Возвращаем значение, если оно не null
                    return !value ? null : $"https://avatars.yandex.net/get-yapic/{picture.GetString()}/islands-75";
                });
            })

            // Добавляет аутентификацию через GitHub
            .AddGitHub(options =>
            {
                // Запрашиваем область user
                options.Scope.Add("read:user");

                // Запрашиваем область email
                options.Scope.Add("user:email");

                // Id oauth приложения
                options.ClientId = githubOauth.Client;

                // Secret oauth приложения
                options.ClientSecret = githubOauth.Secret;

                // Добавляем маппинг для клайма photo:link
                options.ClaimActions.MapCustomJson("photo:link", user =>
                {
                    // Получаем значение с ссылкой на фото профиля
                    var value = user.TryGetProperty("avatar_url", out var picture);

                    // Возвращаем значение, если оно не null
                    return !value ? null : picture.GetString();
                });
            })

            // Добавляет аутентификацию через Google
            .AddGoogle(options =>
            {
                // Id oauth приложения
                options.ClientId = googleOauth.Client;

                // Secret oauth приложения
                options.ClientSecret = googleOauth.Secret;
                
                // Добавляем маппинг для клайма photo:link
                options.ClaimActions.MapCustomJson("photo:link", user =>
                {
                    // Получаем значение с ссылкой на фото профиля
                    var value = user.TryGetProperty("picture", out var picture);

                    // Возвращаем значение, если оно не null
                    return !value ? null : picture.GetString();
                });
            })

            // Добавляет аутентификацию через Microsoft
            .AddMicrosoftAccount(options =>
            {
                // Id oauth приложения
                options.ClientId = microsoftOauth.Client;

                // Secret oauth приложения
                options.ClientSecret = microsoftOauth.Secret;
            })

            // Добавляет аутентификацию через X
            .AddTwitter("X", "X", options =>
            {
                // Пользователь oauth приложения
                options.ConsumerKey = xOauth.Client;

                // Secret oauth приложения
                options.ConsumerSecret = xOauth.Secret;

                // Указываем, что хотим запросить дополнительные данные
                options.RetrieveUserDetails = true;
            })

            // Добавляет аутентификацию через VK ID
            .AddVkId(options =>
            {
                // Id oauth приложения
                options.ClientId = vkOauth.Client;

                // Secret oauth приложения
                options.ClientSecret = vkOauth.Secret;
            });
    }
}