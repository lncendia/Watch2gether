using Overoom.Domain.Rooms.BaseRoom.Exceptions;

namespace Overoom.Domain.Rooms.BaseRoom.ValueObjects;

public class Message
{
    internal Message(int viewerId, string text)
    {
        var count = text.Count(t => t != ' ' && t != '\n');
        if (count == 0 || text.Length > 1000) throw new MessageLengthException();
        ViewerId = viewerId;
        Text = text.Replace('\n', ' ');
    }

    public int ViewerId { get; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public string Text { get; }
}