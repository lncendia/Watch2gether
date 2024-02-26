using System.ComponentModel.DataAnnotations;

namespace PJMS.AuthService.Web.Settings.InputModels;

/// <summary>
/// Модель смены пароля
/// </summary>
public class ChangePasswordInputModel
{
    /// <summary>
    /// Старый пароль
    /// </summary>
    [DataType(DataType.Password)]
    [Display(Name = "OldPassword",
        ResourceType = typeof(Resources.Settings.InputModels.ChangePasswordInputModel))]
    public string? OldPassword { get; init; }

    /// <summary>
    /// Новый пароль
    /// </summary>
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Settings.InputModels.ChangePasswordInputModel))]
    [DataType(DataType.Password)]
    [Display(Name = "NewPassword",
        ResourceType = typeof(Resources.Settings.InputModels.ChangePasswordInputModel))]
    public string? NewPassword { get; init; }

    /// <summary>
    /// Подтверждение пароля
    /// </summary>
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Settings.InputModels.ChangePasswordInputModel))]
    [DataType(DataType.Password)]
    [Display(Name = "NewPasswordConfirm",
        ResourceType = typeof(Resources.Settings.InputModels.ChangePasswordInputModel))]
    [Compare("NewPassword", ErrorMessageResourceName = "NewPasswordConfirmError",
        ErrorMessageResourceType = typeof(Resources.Settings.InputModels.ChangePasswordInputModel))]
    public string? NewPasswordConfirm { get; init; }

    /// <summary>
    /// Url адрес для возврата после прохождения аутентификации
    /// </summary>
    public string ReturnUrl { get; init; } = "/";
}