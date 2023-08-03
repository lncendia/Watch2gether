using Overoom.Application.Abstractions.MovieApi.DTOs;

namespace Overoom.Application.Services.FilmsLoading;

internal class FilmData
{
    public FilmData(Film film, FilmStaff filmStaff, IReadOnlyCollection<Season>? filmSeasons,
        IReadOnlyCollection<Cdn> filmCdns)
    {
        Film = film;
        FilmStaff = filmStaff;
        FilmSeasons = filmSeasons;
        FilmCdns = filmCdns;
    }

    public Film Film { get; }
    public FilmStaff FilmStaff { get; }
    public IReadOnlyCollection<Season>? FilmSeasons { get; }
    public IReadOnlyCollection<Cdn> FilmCdns { get; }
}