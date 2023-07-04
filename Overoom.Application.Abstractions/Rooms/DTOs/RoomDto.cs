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
    public IReadOnlyCollection<MessageDto> Messages { get; }
    protected readonly IReadOnlyCollection<ViewerDto> Viewers;
}

public class MessageDto
{
    public MessageDto(string text, DateTime createdAt, int viewerId, Uri avatarUri, string username)
    {
        Text = text;
        CreatedAt = createdAt;
        ViewerId = viewerId;
        AvatarUri = avatarUri;
        Username = username;
    }

    public string Username { get; }
    public int ViewerId { get; }
    public Uri AvatarUri { get; }
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