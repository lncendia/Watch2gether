using Room.Domain.BaseRoom.Exceptions;

namespace Room.Domain.BaseRoom.ValueObjects;

public class Message
{
    public Message(Guid userId, string text)
    {
        var count = text.Count(t => t != ' ' && t != '\n');
        if (count == 0 || text.Length > 1000) throw new MessageLengthException();
        Text = text.Replace('\n', ' ');
        ViewerId = userId;
    }

    public Guid ViewerId { get; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public string Text { get; }
}