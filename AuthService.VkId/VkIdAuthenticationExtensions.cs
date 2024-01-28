using Microsoft.AspNetCore.Authentication;

namespace AuthService.VkId;

/// <summary> 
/// Методы расширения для добавления возможностей аутентификации ВКонтакте в конвейер обработки HTTP-запросов. 
/// </summary> 
public static class VkIdAuthenticationExtensions
{
    /// <summary> 
    /// Добавляет <see cref="VkIdAuthenticationHandler"/> в указанный 
    /// <see cref="AuthenticationBuilder"/>, что позволяет использовать возможности аутентификации ВКонтакте. 
    /// </summary> 
    /// <param name="builder">Построитель аутентификации.</param> 
    /// <returns>Построитель аутентификации.</returns> 
    public static AuthenticationBuilder AddVkontakte(this AuthenticationBuilder builder)
    {
        return builder.AddVkId(VkIdAuthenticationDefaults.AuthenticationScheme, _ => { });
    }

    /// <summary> 
    /// Добавляет <see cref="VkIdAuthenticationHandler"/> в указанный 
    /// <see cref="AuthenticationBuilder"/>, что позволяет использовать возможности аутентификации ВКонтакте. 
    /// </summary> 
    /// <param name="builder">Построитель аутентификации.</param> 
    /// <param name="configuration">Делегат, используемый для настройки параметров ВКонтакте.</param> 
    /// <returns>Построитель аутентификации.</returns> 
    public static AuthenticationBuilder AddVkId(
        this AuthenticationBuilder builder,
        Action<VkIdAuthenticationOptions> configuration)
    {
        return builder.AddVkId(VkIdAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary> 
    /// Добавляет <see cref="VkIdAuthenticationHandler"/> в указанный 
    /// <see cref="AuthenticationBuilder"/>, что позволяет использовать возможности аутентификации ВКонтакте. 
    /// </summary> 
    /// <param name="builder">Построитель аутентификации.</param> 
    /// <param name="scheme">Схема аутентификации, связанная с данным экземпляром.</param> 
    /// <param name="configuration">Делегат, используемый для настройки параметров ВКонтакте.</param> 
    /// <returns>Построитель аутентификации.</returns> 
    public static AuthenticationBuilder AddVkId(
        this AuthenticationBuilder builder,
        string scheme,
        Action<VkIdAuthenticationOptions> configuration)
    {
        return builder.AddVkId(scheme, VkIdAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary> 
    /// Добавляет <see cref="VkIdAuthenticationHandler"/> в указанный 
    /// <see cref="AuthenticationBuilder"/>, что позволяет использовать возможности аутентификации ВКонтакте. 
    /// </summary> 
    /// <param name="builder">Построитель аутентификации.</param> 
    /// <param name="scheme">Схема аутентификации, связанная с данным экземпляром.</param> 
    /// <param name="caption">Дополнительное отображаемое имя, связанное с данным экземпляром.</param> 
    /// <param name="configuration">Делегат, используемый для настройки параметров ВКонтакте.</param> 
    /// <returns>Построитель аутентификации.</returns> 
    public static AuthenticationBuilder AddVkId(
        this AuthenticationBuilder builder,
        string scheme,
        string caption,
        Action<VkIdAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<VkIdAuthenticationOptions, VkIdAuthenticationHandler>(scheme, caption, configuration);
    }
}