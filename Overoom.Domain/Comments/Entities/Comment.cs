using Overoom.Domain.Abstractions;
using Overoom.Domain.Comments.Exceptions;

namespace Overoom.Domain.Comments.Entities;

public class Comment : AggregateRoot
{
    public Comment(Guid filmId, Guid userId, string text)
    {
        FilmId = filmId;
        UserId = userId;
        if (string.IsNullOrEmpty(text) || text.Length > 1000) throw new TextLengthException();
        Text = text;
    }

    public Guid FilmId { get; }
    public Guid? UserId { get; }

    public string Text { get; }

    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}