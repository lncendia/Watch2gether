using Overoom.Domain.Room.BaseRoom.Exceptions;

namespace Overoom.Domain.Room.BaseRoom.ValueObject;

public class Message
{
    public Message(int viewerId, string text, Guid roomId)
    {
        if (string.IsNullOrEmpty(text) || text.Length > 1000) throw new MessageLengthException();
        ViewerId = viewerId;
        Text = text;
        RoomId = roomId;
    }

    public int ViewerId { get; }
    public Guid RoomId { get; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public string Text { get; }
}