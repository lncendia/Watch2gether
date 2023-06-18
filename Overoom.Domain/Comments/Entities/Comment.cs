using Overoom.Domain.Abstractions;

namespace Overoom.Domain.Comments.Entities;

public class Comment : AggregateRoot
{
    public Comment(Guid filmId, Guid userId, string text)
    {
        FilmId = filmId;
        UserId = userId;
        Text = text;
    }

    public Guid FilmId { get; }
    public Guid? UserId { get; }
    public string Text { get; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}