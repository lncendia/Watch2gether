using AuthService.Infrastructure.Web.Registration.InputModels;

namespace AuthService.Infrastructure.Web.Registration.ViewModels;

/// <summary>
/// Вью-модель регистрации аккаунта
/// </summary>
public class RegistrationViewModel : RegistrationInputModel
{
    /// <summary>
    /// Внешние поставщики
    /// </summary>
    public required string[] ExternalProviders { get; init; }
}