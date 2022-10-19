namespace Watch2gether.Application.Abstractions.DTO.Rooms;

public abstract class RoomDto
{
    protected RoomDto(IEnumerable<MessageDto> messages, IEnumerable<ViewerDto> viewers, Guid ownerId)
    {
        Messages = messages.ToList();
        Viewers = viewers.ToList();
        OwnerId = ownerId;
    }

    public Guid OwnerId { get; }
    public readonly List<MessageDto> Messages;
    protected readonly List<ViewerDto> Viewers;
}

public abstract class MessageDto
{
    protected MessageDto(string text, DateTime createdAt, ViewerDto viewer)
    {
        Text = text;
        CreatedAt = createdAt;
        Viewer = viewer;
    }

    protected readonly ViewerDto Viewer;
    public DateTime CreatedAt { get; }
    public string Text { get; }
}

public abstract class ViewerDto
{
    protected ViewerDto(string username, Guid id, string avatarUrl, TimeSpan time, bool onPause)
    {
        Username = username;
        Id = id;
        AvatarUrl = avatarUrl;
        Time = time;
        OnPause = onPause;
    }

    public Guid Id { get; }
    public string Username { get; }
    public string AvatarUrl { get; }
    public bool OnPause { get; }
    public TimeSpan Time { get; }
}