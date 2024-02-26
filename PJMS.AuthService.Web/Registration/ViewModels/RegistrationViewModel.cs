using PJMS.AuthService.Web.Registration.InputModels;

namespace PJMS.AuthService.Web.Registration.ViewModels;

/// <summary>
/// Вью-модель регистрации аккаунта
/// </summary>
public class RegistrationViewModel : RegistrationInputModel
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