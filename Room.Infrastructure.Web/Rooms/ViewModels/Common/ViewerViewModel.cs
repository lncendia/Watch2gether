namespace Room.Infrastructure.Web.Rooms.ViewModels.Common;

public abstract class ViewerViewModel
{
    public required Guid Id { get; init; }
    public required string Username { get; init; }
    public string? PhotoUrl { get; init; }
    public required bool Pause { get; init; }
    public required bool FullScreen { get; init; }

    public required bool Online { get; init; }
    public required int Second { get; init; }
    public required AllowsViewModel Allows { get; init; }
}