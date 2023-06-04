using Overoom.Application.Abstractions.Film.DTOs.FilmCatalog;
using Overoom.Application.Abstractions.Film.DTOs.Playlist;

namespace Overoom.Application.Abstractions.Film.Interfaces;

public interface IFilmMapper
{
    public FilmDto MapFilm(Domain.Film.Entities.Film film);

    public FilmShortDto MapFilmShort(Domain.Film.Entities.Film film);

    public PlaylistDto MapPlaylist(Domain.Playlist.Entities.Playlist playlist);
}