using PJMS.AuthService.Web.Account.InputModels;

namespace PJMS.AuthService.Web.Account.ViewModels;

/// <summary>
/// Вью-модель входа в систему
/// </summary>
public class LoginViewModel : LoginInputModel
{
    /// <summary>
    /// Включить локальный вход
    /// </summary>
    public required bool EnableLocalLogin { get; init; }

    /// <summary>
    /// Внешние поставщики
    /// </summary>
    public required string[] ExternalProviders { get; init; }
}