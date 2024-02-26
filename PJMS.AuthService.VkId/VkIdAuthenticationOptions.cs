using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace PJMS.AuthService.VkId;

/// <summary> 
/// Определяет набор параметров, используемых <see cref="VkIdAuthenticationHandler"/>. 
/// </summary> 
public class VkIdAuthenticationOptions : OAuthOptions
{
    /// <summary> 
    /// Создает новый экземпляр класса <see cref="VkIdAuthenticationOptions"/>. 
    /// </summary> 
    public VkIdAuthenticationOptions()
    {
        // Установка значения издателя утверждений 
        ClaimsIssuer = VkIdAuthenticationDefaults.Issuer;

        // Установка пути обратного вызова 
        CallbackPath = VkIdAuthenticationDefaults.CallbackPath;

        // Установка эндпоинта авторизации 
        AuthorizationEndpoint = VkIdAuthenticationDefaults.AuthorizationEndpoint;

        // Установка эндпоинта получения токена 
        TokenEndpoint = VkIdAuthenticationDefaults.TokenEndpoint;

        // Установка эндпоинта получения информации о пользователе 
        UserInformationEndpoint = VkIdAuthenticationDefaults.UserInformationEndpoint;

        // Маппинг JSON-ключа "id" на утверждение "NameIdentifier" 
        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");

        // Маппинг JSON-ключа "first_name" на утверждение "GivenName" 
        ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");

        // Маппинг JSON-ключа "last_name" на утверждение "Surname" 
        ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");

        // Маппинг JSON-ключа "email" на утверждение "Email" 
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

        // Маппинг JSON-ключа "photo" на утверждение "PhotoUrl" 
        ClaimActions.MapJsonKey(Claims.PhotoUrl, "photo");

        // Маппинг JSON-ключа "photo_rec" на утверждение "ThumbnailUrl" 
        ClaimActions.MapJsonKey(Claims.ThumbnailUrl, "photo_rec");

        ClaimActions.MapCustomJson(
            ClaimTypes.Name,
            user =>
            {
                //получаем значение name
                if (!user.TryGetProperty("first_name", out var firstName)) return null;

                if (!user.TryGetProperty("last_name", out var lastName)) return null;

                return firstName.GetString() + ' ' + lastName.GetString();
            });
    }

    /// <summary> 
    /// Возвращает список полей, которые необходимо получить из эндпоинта информации о пользователе. 
    /// См. https://vk.com/dev/fields для получения дополнительной информации. 
    /// </summary> 
    public ISet<string> Fields { get; } = new HashSet<string>
    {
        "id",
        "first_name",
        "last_name",
        "photo_rec",
        "photo",
        "hash"
    };

    /// <summary> 
    /// Версия API ВКонтакте, используемая для запросов. 
    /// </summary> 
    public string ApiVersion { get; set; } = VkIdAuthenticationDefaults.ApiVersion;
}