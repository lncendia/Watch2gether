namespace Watch2gether.Domain.Rooms.ValueObject;

public class Message
{
    public Message(Guid viewerId, string text, Guid roomId)
    {
        ViewerId = viewerId;
        Text = text;
        RoomId = roomId;
    }

    public Guid ViewerId { get; }
    public Guid RoomId { get; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public string Text { get; init; } //TODO: add validation
}