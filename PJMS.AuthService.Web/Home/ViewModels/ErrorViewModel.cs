namespace PJMS.AuthService.Web.Home.ViewModels;

/// <summary>
/// ViewModel ошибки
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Получаем или устанавливаем сообщение ошибки.
    /// </summary>
    public required string Message { get; init; } 
    
    /// <summary>
    /// Получаем или устанавливаем идентификатор запроса.
    /// </summary>
    public string? RequestId { get; init; }
    
    /// <summary>
    /// Возвращает true - если есть идентификатор запроса.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    
    /// <summary>
    /// URL возврата
    /// </summary>
    public required string ReturnUrl { get; init; }
}