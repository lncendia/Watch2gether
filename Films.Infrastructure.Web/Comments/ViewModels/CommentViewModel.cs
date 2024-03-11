namespace Films.Infrastructure.Web.Comments.ViewModels;

public class CommentViewModel
{
    public required Guid Id { get; init; }
    public required string Username { get; init; }
    public required string Text { get; init; }
    public string? AvatarUrl { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required bool IsUserComment { get; init; }
}