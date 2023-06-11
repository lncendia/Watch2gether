using Overoom.Application.Abstractions.Film.Kinopoisk.DTOs;
using Overoom.Application.Abstractions.Film.Load.DTOs;
using Overoom.Application.Abstractions.Film.Load.Interfaces;

namespace Overoom.Application.Services.Film.Load;

public class FilmKpMapper : IFilmKpMapper
{
    public FilmDto Map(Abstractions.Film.Kinopoisk.DTOs.Film film, FilmStaff staff,
        IReadOnlyCollection<Season>? seasons)
    {
        throw new NotImplementedException();
    }
}