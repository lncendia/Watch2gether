namespace PJMS.AuthService.Web.TwoFactor.InputModels;

/// <summary>
/// Модель для прохождения 2FA
/// </summary>
public class LoginTwoStepInputModel : TwoFactorAuthenticateInputModel
{
    /// <summary>
    /// Флаг необходимости запомнить пользователя
    /// </summary>
    public bool RememberMe { get; init; }
    
    /// <summary>
    /// Url адрес возврата после прохождения 2FA
    /// </summary>
    public string ReturnUrl { get; init; } = "/";
}