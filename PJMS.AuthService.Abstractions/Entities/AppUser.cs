using Microsoft.AspNetCore.Identity;
using PJMS.AuthService.Abstractions.Accessories;
using PJMS.AuthService.Abstractions.Enums;

namespace PJMS.AuthService.Abstractions.Entities;

/// <summary>
/// Модель пользователя
/// </summary>
public sealed class AppUser : IdentityUser<Guid>
{
    /// <summary>
    /// Время регистрации
    /// </summary>
    public required DateTime TimeRegistration { get; set; }

    /// <summary>
    /// Время последней аутентификации
    /// </summary>
    public required DateTime TimeLastAuth { get; set; }

    /// <summary>
    /// Язык локализации
    /// </summary>
    public required Localization Locale { get; set; }

    /// <summary>
    /// Миниатюра
    /// </summary>
    public Uri? Thumbnail { get; set; }

    /// <summary>
    /// Страна
    /// </summary>
    public string Country => Locale.GetLocalizationAsString();
}