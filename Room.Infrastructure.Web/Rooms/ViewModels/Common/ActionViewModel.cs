namespace Room.Infrastructure.Web.Rooms.ViewModels.Common;

public class ActionViewModel
{
    public required Guid Initiator { get; init; }
    public required Guid Target { get; init; }
}