namespace Overoom.Application.Abstractions.Comments.DTOs;

public class CommentDto
{
    public CommentDto(Guid id, string text, DateTime createdAt, string username, Uri avatarUri)
    {
        Id = id;
        Text = text;
        CreatedAt = createdAt;
        Username = username;
        AvatarUri = avatarUri;
    }

    public Guid Id { get; }
    public string Text { get; }
    public DateTime CreatedAt { get; }
    public string Username { get; }
    public Uri AvatarUri { get; }
}