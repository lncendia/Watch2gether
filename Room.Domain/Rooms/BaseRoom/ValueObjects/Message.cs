using Room.Domain.Rooms.BaseRoom.Exceptions;

namespace Room.Domain.Rooms.BaseRoom.ValueObjects;

public class Message
{
    public Message(Guid userId, string text)
    {
        var count = text.Count(t => t != ' ' && t != '\n');
        if (count == 0 || text.Length > 1000) throw new MessageLengthException();
        Text = text.Replace('\n', ' ');
        UserId = userId;
    }

    public Guid UserId { get; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public string Text { get; }
}