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
    ///  Url адрес возврата.
    /// </summary>
    public string ReturnUrl { get; init; } = "/";
    
    /// <summary>
    /// Номер вкладки, которая должна быть раскрыта.
    /// </summary>
    public int ExpandElem { get; init; } = 1;
}