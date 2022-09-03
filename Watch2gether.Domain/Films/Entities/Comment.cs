namespace Watch2gether.Domain.Films.Entities;

public class Comment
{
    public Comment(Guid id, Guid userId, Guid filmId, string text)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        FilmId = filmId;
        Text = text;
    }

    public Guid Id { get; }
    public string Text { get; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public Guid? UserId { get; }
    public Guid FilmId { get; }
}