using AuthService.Infrastructure.Web.Account.InputModels;

namespace AuthService.Infrastructure.Web.Account.ViewModels;

/// <summary>
/// Вью-модель входа в систему
/// </summary>
public class LoginViewModel : LoginInputModel
{
    /// <summary>
    /// Внешние поставщики
    /// </summary>
    public required string[] ExternalProviders { get; init; }
}