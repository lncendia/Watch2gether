using Room.Domain.Abstractions;
using Room.Domain.Messages.Messages.Exceptions;
using Room.Domain.Rooms.Rooms.Entities;

namespace Room.Domain.Messages.Messages;

public abstract class Message : AggregateRoot
{
    protected Message(IEnumerable<Viewer> viewers, Guid roomId, Guid userId, string text)
    {
        var count = text.Count(t => t != ' ' && t != '\n');
        if (count == 0 || text.Length > 1000) throw new MessageLengthException();
        Text = text.Replace('\n', ' ');
        UserId = userId;
        RoomId = roomId;
    }

    public Guid RoomId { get; }
    public Guid UserId { get; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public string Text { get; }
}