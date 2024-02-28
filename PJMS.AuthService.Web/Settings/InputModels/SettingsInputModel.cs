namespace PJMS.AuthService.Web.Settings.InputModels;

/// <summary>
/// Модель страницы настроек.
/// </summary>
public class SettingsInputModel
{
    /// <summary>
    /// Сообщение для пользователя.
    /// </summary>
    public string? Message { get; init; }
    
    /// <summary>
    /// Номер вкладки, которая должна быть раскрыта.
    /// </summary>
    public int ExpandElem { get; init; } = 1;
}