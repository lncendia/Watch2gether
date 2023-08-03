namespace Overoom.Application.Abstractions.Rooms.DTOs;

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