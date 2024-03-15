namespace Room.Infrastructure.Web.Rooms.ViewModels.Messages;

public class MessageViewModel
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required string Text { get; init; }
    public required DateTime CreatedAt { get; init; }
}