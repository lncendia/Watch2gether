using System.ComponentModel.DataAnnotations;

namespace PJMS.AuthService.Web.Settings.InputModels;

/// <summary>
/// Модель ввода для изменения имени.
/// </summary>
public class ChangeNameInputModel
{
    /// <summary>
    /// Получает или задает имя пользователя.
    /// </summary>
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Settings.InputModels.ChangeNameInputModel))]
    [Display(Name = "Username",
        ResourceType = typeof(Resources.Settings.InputModels.ChangeNameInputModel))]
    [MaxLength(40, ErrorMessageResourceName = "MaxLength",
        ErrorMessageResourceType = typeof(Resources.Settings.InputModels.ChangeNameInputModel))]
    public string? Username { get; init; }

    /// <summary>
    /// Получает или задает URL возврата.
    /// </summary>
    public string ReturnUrl { get; init; } = "/";
}