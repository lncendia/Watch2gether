using Overoom.Application.Abstractions.Comments.DTOs;
using Overoom.Application.Abstractions.Films.Catalog.DTOs;
using Overoom.Application.Abstractions.Films.Playlist.DTOs;
using Overoom.WEB.Contracts.Films;
using Overoom.WEB.Models.Films;

namespace Overoom.WEB.Mappers.Abstractions;

public interface IFilmMapper
{
    public FilmSearchQueryDto Map(FilmsSearchParameters model);
    public PlaylistSearchQueryDto Map(PlaylistSearchViewModel model);
    public FilmViewModel Map(FilmDto film);
    public PlaylistViewModel Map(PlaylistDto film);
    public FilmShortViewModel Map(FilmShortDto film);
    public CommentViewModel Map(CommentDto comment);
}