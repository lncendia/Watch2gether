using Room.Domain.Abstractions;
using Room.Domain.Extensions;
using Room.Domain.Rooms.Rooms.Entities;

namespace Room.Domain.Messages.Messages;

public abstract class Message : AggregateRoot
{
    private const int MaxTextLength = 1000;

    protected Message(IEnumerable<Viewer> viewers, Guid roomId, Guid userId, string text)
    {
        Text = text
            .Replace('\n', ' ')
            .ValidateLength(MaxTextLength);
        
        UserId = userId;
        RoomId = roomId;
    }

    public Guid RoomId { get; }
    public Guid UserId { get; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public string Text { get; }
}