namespace AuthService.Infrastructure.Web.Settings.ViewModels;

/// <summary>
/// Внешний поставщик
/// </summary>
public class ExternalProvider
{
    /// <summary>
    /// Имя
    /// </summary>
    public required string DisplayName { get; init; }

    /// <summary>
    /// Схема аутентификации
    /// </summary>
    public required string AuthenticationScheme { get; init; }

    /// <summary>
    /// Связана ли схема с пользователем
    /// </summary>
    public required bool IsAssociated { get; init; }
}