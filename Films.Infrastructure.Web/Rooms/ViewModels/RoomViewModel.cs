namespace Films.Infrastructure.Web.Rooms.ViewModels;

public abstract class RoomViewModel
{
    public required Guid Id { get; init; }
    public required int ViewersCount { get; init; }
    public required bool IsCodeNeeded { get; init; }
}