using PJMS.AuthService.Web.Settings.InputModels;

namespace PJMS.AuthService.Web.Settings.ViewModels;

/// <summary>
/// Модель представления для изменения аватара.
/// </summary>
public class ChangeAvatarViewModel : ChangeAvatarInputModel
{
    /// <summary>
    /// Получает или задает ссылку на миниатюру.
    /// </summary>
    public Uri? Thumbnail { get; init; }
}