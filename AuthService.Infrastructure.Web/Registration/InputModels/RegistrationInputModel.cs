using System.ComponentModel.DataAnnotations;

namespace AuthService.Infrastructure.Web.Registration.InputModels;

/// <summary>
/// Модель регистрации аккаунта
/// </summary>
public class RegistrationInputModel
{
    /// <summary>
    /// Почта пользователя
    /// </summary>
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Registration.InputModels.RegistrationInputModel))]
    [EmailAddress(ErrorMessageResourceName = "ValidEmail",
        ErrorMessageResourceType = typeof(Resources.Registration.InputModels.RegistrationInputModel))]
    [Display(Name = "Email",
        ResourceType = typeof(Resources.Registration.InputModels.RegistrationInputModel))]
    public string? Email { get; set; }
    
    /// <summary>
    /// Логин (имя) пользователя
    /// </summary>
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Registration.InputModels.RegistrationInputModel))]
    [RegularExpression("^[A-Za-zА-Яа-яЁё0-9.@\\-_+ ]+$", ErrorMessageResourceName = "ValidUsername",
        ErrorMessageResourceType = typeof(Resources.Registration.InputModels.RegistrationInputModel))]
    [Display(Name = "Username",
        ResourceType = typeof(Resources.Registration.InputModels.RegistrationInputModel))]
    [MaxLength(20, ErrorMessageResourceName = "MaxLength",
        ErrorMessageResourceType = typeof(Resources.Registration.InputModels.RegistrationInputModel))]
    [MinLength(3, ErrorMessageResourceName = "MinLength",
        ErrorMessageResourceType = typeof(Resources.Registration.InputModels.RegistrationInputModel))]
    public string? Username { get; set; }

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