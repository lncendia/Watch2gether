using Overoom.Domain.Room.BaseRoom.Exceptions;

namespace Overoom.Domain.Room.BaseRoom.ValueObject;

public class Message
{
    internal Message(int viewerId, string text)
    {
        if (string.IsNullOrEmpty(text) || text.Length > 1000) throw new MessageLengthException();
        ViewerId = viewerId;
        Text = text;
    }

    public int ViewerId { get; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public string Text { get; }
}