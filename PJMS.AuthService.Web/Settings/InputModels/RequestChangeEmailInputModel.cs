using System.ComponentModel.DataAnnotations;

namespace PJMS.AuthService.Web.Settings.InputModels;

/// <summary>
/// Модель запроса смены почты
/// </summary>
public class RequestChangeEmailInputModel
{
    /// <summary>
    /// Почта пользователя
    /// </summary>
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Settings.InputModels.RequestChangeEmailInputModel))]
    [EmailAddress(ErrorMessageResourceName = "ValidEmail",
        ErrorMessageResourceType = typeof(Resources.Settings.InputModels.RequestChangeEmailInputModel))]
    [Display(Name = "Email",
        ResourceType = typeof(Resources.Settings.InputModels.RequestChangeEmailInputModel))]
    public string? Email { get; init; }

    /// <summary>
    /// Url адрес для возврата после прохождения аутентификации
    /// </summary>
    public string ReturnUrl { get; init; } = "/";
}