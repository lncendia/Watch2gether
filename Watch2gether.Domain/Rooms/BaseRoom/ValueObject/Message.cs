using Watch2gether.Domain.Rooms.BaseRoom.Exceptions;

namespace Watch2gether.Domain.Rooms.BaseRoom.ValueObject;

public class Message
{
    public Message(Guid viewerId, string text, Guid roomId)
    {
        if (string.IsNullOrEmpty(text) || text.Length > 1000) throw new MessageLengthException();
        ViewerId = viewerId;
        Text = text;
        RoomId = roomId;
    }

    public Guid ViewerId { get; }
    public Guid RoomId { get; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public string Text { get; }
}