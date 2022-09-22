namespace Watch2gether.Domain.Comments;

public class Comment
{
    public Comment(Guid filmId, Guid userId, string text)
    {
        Id = Guid.NewGuid();
        FilmId = filmId;
        UserId = userId;
        Text = text;
    }

    public Guid Id { get; }
    public Guid FilmId { get; }
    public Guid UserId { get; }
    public string Text { get; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}