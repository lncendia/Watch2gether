using Overoom.Application.Abstractions.Comments.DTOs;
using Overoom.Application.Abstractions.Films.Catalog.DTOs;
using Overoom.Application.Abstractions.Films.Playlist.DTOs;
using Overoom.WEB.Contracts.Films;
using Overoom.WEB.Mappers.Abstractions;
using Overoom.WEB.Models.Films;

namespace Overoom.WEB.Mappers;

public class FilmMapper : IFilmMapper
{
    public FilmSearchQueryDto Map(FilmsSearchParameters model) =>
        new(model.Query, model.MinYear, model.MaxYear, model.Genre, model.Country, model.Person, model.Type,
            model.PlaylistId, model.SortBy, model.Page, model.InverseOrder);

    public PlaylistSearchQueryDto Map(PlaylistsSearchParameters model) =>
        new(model.Name, model.SortBy, model.Page, model.InverseOrder);

    public FilmViewModel Map(FilmDto film)
    {
        var cdnList = film.CdnList.Select(x => new CdnViewModel(x.Type, x.Voices, x.Quality)).ToList();
        return new FilmViewModel(film.Id, film.Name, film.Year, film.Type, film.PosterUri, film.Description,
            film.RatingKp, film.Directors, film.ScreenWriters, film.Genres, film.Countries, film.Actors,
            film.CountSeasons, film.CountEpisodes, cdnList);
    }

    public PlaylistViewModel Map(PlaylistDto playlist) =>
        new(playlist.Id, playlist.Name, playlist.Genres, playlist.Description, playlist.PosterUri, playlist.Updated);

    public PlaylistShortViewModel Map(PlaylistShortDto playlist) => new(playlist.Id, playlist.Name, playlist.PosterUri);

    public FilmShortViewModel Map(FilmShortDto film) => new(film.Id, film.Name, film.PosterUri, film.Rating,
        film.ShortDescription, film.Year, film.Type, film.CountSeasons, film.Genres);

    public CommentViewModel Map(CommentDto comment) =>
        new(comment.Username, comment.Text, comment.CreatedAt, comment.AvatarUri);
}