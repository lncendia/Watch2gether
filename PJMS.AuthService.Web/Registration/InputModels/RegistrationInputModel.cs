using System.ComponentModel.DataAnnotations;

namespace PJMS.AuthService.Web.Registration.InputModels;

/// <summary>
/// Модель регистрации аккаунта
/// </summary>
public class RegistrationInputModel
{
    /// <summary>
    /// Логин (имя) пользователя
    /// </summary>
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Registration.InputModels.RegistrationInputModel))]
    [EmailAddress(ErrorMessageResourceName = "ValidEmail",
        ErrorMessageResourceType = typeof(Resources.Registration.InputModels.RegistrationInputModel))]
    [Display(Name = "Email",
        ResourceType = typeof(Resources.Registration.InputModels.RegistrationInputModel))]
    public string? Email { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Registration.InputModels.RegistrationInputModel))]
    [DataType(DataType.Password)]
    [Display(Name = "Password",
        ResourceType = typeof(Resources.Registration.InputModels.RegistrationInputModel))]
    public string? Password { get; set; }

    /// <summary>
    /// Подтверждение пароля
    /// </summary>
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Registration.InputModels.RegistrationInputModel))]
    [DataType(DataType.Password)]
    [Display(Name = "PasswordConfirm",
        ResourceType = typeof(Resources.Registration.InputModels.RegistrationInputModel))]
    [Compare("Password", ErrorMessageResourceName = "PasswordConfirmError",
        ErrorMessageResourceType = typeof(Resources.Registration.InputModels.RegistrationInputModel))]
    public string? PasswordConfirm { get; set; }

    /// <summary>
    /// Флаг необходимости запомнить логин
    /// </summary>
    [Display(Name = "Remember",
        ResourceType = typeof(Resources.Registration.InputModels.RegistrationInputModel))]
    public bool RememberLogin { get; init; }

    /// <summary>
    /// Url адрес для возврата после прохождения аутентификации
    /// </summary>
    public string ReturnUrl { get; init; } = "/";
}