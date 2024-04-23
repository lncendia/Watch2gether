using System.ComponentModel.DataAnnotations;

namespace PJMS.AuthService.Web.Account.InputModels;

/// <summary>
/// Модель запроса восстановления пароля
/// </summary>
public class ResetPasswordInputModel
{
    /// <summary>
    /// Модель ввода для сброса пароля.
    /// </summary>
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Account.InputModels.ResetPasswordInputModel))]
    [EmailAddress(ErrorMessageResourceName = "ValidEmail",
        ErrorMessageResourceType = typeof(Resources.Account.InputModels.ResetPasswordInputModel))]
    [Display(Name = "Email",
        ResourceType = typeof(Resources.Account.InputModels.ResetPasswordInputModel))]
    public string? Email { get; init; }

    /// <summary>
    /// URL для возврата.
    /// </summary>
    public string ReturnUrl { get; init; } = "/";
}