namespace Room.Infrastructure.Web.Rooms.ViewModels.Messages;

public class MessagesViewModel
{
    public required IEnumerable<MessageViewModel> Messages { get; init; }
    public required int Count { get; init; }
}