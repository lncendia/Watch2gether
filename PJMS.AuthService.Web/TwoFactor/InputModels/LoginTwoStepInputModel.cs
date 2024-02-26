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
}