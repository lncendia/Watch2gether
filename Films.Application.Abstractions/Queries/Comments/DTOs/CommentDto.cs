namespace Films.Application.Abstractions.Queries.Comments.DTOs;

public class CommentDto
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required string Text { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string Username { get; init; }
    public required Uri PhotoUrl { get; init; }
}