namespace Overoom.Application.Abstractions.Rooms.DTOs;

public abstract class RoomDto
{
    protected RoomDto(IReadOnlyCollection<MessageDto> messages, IReadOnlyCollection<ViewerDto> viewers, int ownerId)
    {
        Messages = messages;
        Viewers = viewers;
        OwnerId = ownerId;
    }

    public int OwnerId { get; }
    protected readonly IReadOnlyCollection<MessageDto> Messages;
    protected readonly IReadOnlyCollection<ViewerDto> Viewers;
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
    protected ViewerDto(string username, int id, Uri avatarUrl, TimeSpan time, bool onPause)
    {
        Username = username;
        Id = id;
        AvatarUrl = avatarUrl;
        Time = time;
        OnPause = onPause;
    }

    public int Id { get; }
    public string Username { get; }
    public Uri AvatarUrl { get; }
    public bool OnPause { get; }
    public TimeSpan Time { get; }
}