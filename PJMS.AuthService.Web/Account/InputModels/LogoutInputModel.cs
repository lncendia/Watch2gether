namespace PJMS.AuthService.Web.Account.InputModels;

/// <summary>
/// Модель ввода выхода из системы
/// </summary>
public class LogoutInputModel
{
    /// <summary>
    /// Logout Id
    /// </summary>
    public string? LogoutId { get; init; }
}