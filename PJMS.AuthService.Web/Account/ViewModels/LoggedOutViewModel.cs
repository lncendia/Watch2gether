namespace PJMS.AuthService.Web.Account.ViewModels;

/// <summary>
/// Модель представления LoggedOut
/// </summary>
public class LoggedOutViewModel
{
    /// <summary>
    /// Uri перенаправления после выхода из системы
    /// </summary>
    public required string PostLogoutRedirectUri { get; init; }

    /// <summary>
    /// URL-адрес Iframe для выхода
    /// </summary>
    public string? SignOutIframeUrl { get; init; }
}