namespace Room.Infrastructure.Web.Rooms.ViewModels.Common;

public abstract class RoomViewModel<T> where T : ViewerViewModel
{
    public required Guid Id { get; init; }
    public required Guid OwnerId { get; init; }
    public required IReadOnlyCollection<T> Viewers { get; init; }
}