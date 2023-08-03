namespace Overoom.Application.Abstractions.Comments.DTOs;

public class CommentDto
{
    public CommentDto(Guid id, Guid userId, string text, DateTime createdAt, string username, Uri avatarUri)
    {
        Id = id;
        Text = text;
        CreatedAt = createdAt;
        Username = username;
        AvatarUri = avatarUri;
        UserId = userId;
    }

    public Guid Id { get; }
    public Guid UserId { get; }
    public string Text { get; }
    public DateTime CreatedAt { get; }
    public string Username { get; }
    public Uri AvatarUri { get; }
}