using System.ComponentModel.DataAnnotations;

namespace PJMS.AuthService.Web.TwoFactor.InputModels;

/// <summary>
/// Модель установки 2FA для пользователя
/// </summary>
public class SetupTwoFactorInputModel
{
    /// <summary>
    /// Ключ аутентификатора
    /// </summary>
    [Required] public string? AuthenticatorKey { get; init; }

    /// <summary>
    /// Код подтверждения аутентификации
    /// </summary>
    [Display(Name = "Code", ResourceType = typeof(Resources.TwoFactor.InputModels.SetupTwoFactorInputModel))]
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.TwoFactor.InputModels.SetupTwoFactorInputModel))]
    public string? Code { get; set; }
}