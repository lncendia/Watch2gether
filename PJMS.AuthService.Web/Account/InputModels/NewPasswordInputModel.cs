using System.ComponentModel.DataAnnotations;

namespace PJMS.AuthService.Web.Account.InputModels;

/// <summary>
/// Модель установки нового пароля
/// </summary>
public class NewPasswordInputModel
{
    /// <summary>
    /// Модель ввода для нового пароля.
    /// </summary>
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Account.InputModels.NewPasswordInputModel))]
    [DataType(DataType.Password)]
    [Display(Name = "NewPassword",
        ResourceType = typeof(Resources.Account.InputModels.NewPasswordInputModel))]
    public string? NewPassword { get; init; }

    /// <summary>
    /// Подтверждение нового пароля.
    /// </summary>
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Account.InputModels.NewPasswordInputModel))]
    [DataType(DataType.Password)]
    [Display(Name = "NewPasswordConfirm",
        ResourceType = typeof(Resources.Account.InputModels.NewPasswordInputModel))]
    [Compare("NewPassword", ErrorMessageResourceName = "NewPasswordConfirmError",
        ErrorMessageResourceType = typeof(Resources.Account.InputModels.NewPasswordInputModel))]
    public string? PasswordConfirm { get; init; }

    /// <summary>
    /// Адрес электронной почты.
    /// </summary>
    [Required]
    [EmailAddress]
    public string? Email { get; init; }

    /// <summary>
    /// Код.
    /// </summary>
    [Required]
    public string? Code { get; init; }

    /// <summary>
    /// URL для возврата.
    /// </summary>
    public string ReturnUrl { get; init; } = "/";
}