namespace AuthService.Infrastructure.Web.Home.ViewModels;

/// <summary>
/// ViewModel ошибки
/// </summary>
/// <summary>
/// ViewModel ошибки
/// </summary>
public class ErrorViewModel
{
    public required string Message { get; init; } 
    public string? RequestId { get; init; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}