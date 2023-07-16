namespace Overoom.Domain.Users.ValueObjects;

public class FilmNote
{
    internal FilmNote(Guid filmId)
    {
        FilmId = filmId;
        Date = DateTime.UtcNow;
    }

    public Guid FilmId { get; }
    public DateTime Date { get; }
}