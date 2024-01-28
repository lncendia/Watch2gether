using System.ComponentModel.DataAnnotations;

namespace AuthService.Infrastructure.Web.Account.InputModels;

/// <summary>
/// Модель входа в систему
/// </summary>
public class LoginInputModel
{
    /// <summary>
    /// Логин (имя) пользователя
    /// </summary>
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Account.InputModels.LoginInputModel))]
    [EmailAddress(ErrorMessageResourceName = "ValidEmail",
        ErrorMessageResourceType = typeof(Resources.Account.InputModels.LoginInputModel))]
    [Display(Name = "Email",
        ResourceType = typeof(Resources.Account.InputModels.LoginInputModel))]
    public string? Email { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Account.InputModels.LoginInputModel))]
    [DataType(DataType.Password)]
    [Display(Name = "Password",
        ResourceType = typeof(Resources.Account.InputModels.LoginInputModel))]
    public string? Password { get; set; }

    /// <summary>
    /// Флаг необходимости запомнить логин
    /// </summary>
    [Display(Name = "Remember",
        ResourceType = typeof(Resources.Account.InputModels.LoginInputModel))]
    public bool RememberLogin { get; set; }

    /// <summary>
    /// Url адрес для возврата после прохождения аутентификации
    /// </summary>
    public string ReturnUrl { get; init; } = "/";
}