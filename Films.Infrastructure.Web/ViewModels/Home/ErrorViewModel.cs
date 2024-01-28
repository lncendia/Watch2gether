namespace Films.Infrastructure.Web.Models.Home;

public class ErrorViewModel
{
    public ErrorViewModel(string? message, string? requestId)
    {
        Message = message;
        RequestId = requestId;
    }

    public string? Message { get; }
    public string? RequestId { get; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}