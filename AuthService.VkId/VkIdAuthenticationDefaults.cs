using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AuthService.VkId;

/// <summary> 
/// Значения по умолчанию, используемые посредником аутентификации Vkontakte. 
/// </summary> 
public static class VkIdAuthenticationDefaults
{
    /// <summary> 
    /// Значение по умолчанию для <see cref="Microsoft.AspNetCore.Authentication.AuthenticationScheme.Name"/>. 
    /// </summary> 
    public const string AuthenticationScheme = "VkId";

    /// <summary> 
    /// Значение по умолчанию для <see cref="Microsoft.AspNetCore.Authentication.AuthenticationScheme.DisplayName"/>. 
    /// </summary> 
    public const string DisplayName = "VK ID";

    /// <summary> 
    /// Значение по умолчанию для <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>. 
    /// </summary> 
    public const string Issuer = "VkId";

    /// <summary> 
    /// Значение по умолчанию для <see cref="RemoteAuthenticationOptions.CallbackPath"/>. 
    /// </summary> 
    public const string CallbackPath = "/signin-vkid";

    /// <summary> 
    /// Значение по умолчанию для <see cref="OAuthOptions.AuthorizationEndpoint"/>. 
    /// </summary> 
    public const string AuthorizationEndpoint = "https://id.vk.com/auth";

    /// <summary> 
    /// Значение по умолчанию для <see cref="OAuthOptions.TokenEndpoint"/>. 
    /// </summary> 
    public const string TokenEndpoint = "https://api.vk.com/method/auth.exchangeSilentAuthToken";

    /// <summary> 
    /// Значение по умолчанию для <see cref="OAuthOptions.UserInformationEndpoint"/>. 
    /// </summary> 
    public const string UserInformationEndpoint = "https://api.vk.com/method/users.get.json";

    /// <summary> 
    /// Версия API по умолчанию. 
    /// </summary> 
    public const string ApiVersion = "5.131";
}