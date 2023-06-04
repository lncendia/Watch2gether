namespace Overoom.Application.Abstractions.Comment.DTOs;

public class CommentDto
{
    public CommentDto(Guid id, string text, DateTime createdAt, string username, string avatarFileName)
    {
        Id = id;
        Text = text;
        CreatedAt = createdAt;
        Username = username;
        AvatarFileName = avatarFileName;
    }

    public Guid Id { get; }
    public string Text { get; }
    public DateTime CreatedAt { get; }
    public string Username { get; }
    public string AvatarFileName { get; }
}