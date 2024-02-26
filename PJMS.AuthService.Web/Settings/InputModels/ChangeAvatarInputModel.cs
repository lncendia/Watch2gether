using System.ComponentModel.DataAnnotations;

namespace PJMS.AuthService.Web.Settings.InputModels;

/// <summary>
/// Модель ввода для изменения аватара.
/// </summary>
public class ChangeAvatarInputModel
{
    /// <summary>
    /// Получает или задает файл изображения.
    /// </summary>
    [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Settings.InputModels.ChangeAvatarInputModel))]
    public IFormFile? File { get; init; }

    /// <summary>
    /// Получает или задает URL возврата.
    /// </summary>
    public string ReturnUrl { get; init; } = "/";
}