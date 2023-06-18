namespace Overoom.Application.Abstractions.Films.Kinopoisk.DTOs;

public class FilmShort
{
    public FilmShort(long kpId, string? imdbId, string title)
    {
        KpId = kpId;
        ImdbId = imdbId;
        Title = title;
    }

    public long KpId { get; }
    public string? ImdbId { get; }
    public string Title { get; }
}