using Overoom.Application.Abstractions.Content.DTOs;
using Overoom.WEB.Contracts.Content;
using Overoom.WEB.Mappers.Abstractions;
using Overoom.WEB.Models.Content;

namespace Overoom.WEB.Mappers;

public class ContentMapper : IContentMapper
{
    public FilmSearchQuery Map(FilmsSearchParameters model) =>
        new(model.Query, model.MinYear, model.MaxYear, model.Genre, model.Country, model.Person, model.Type,
            model.PlaylistId, model.SortBy, model.Page, model.InverseOrder);

    public PlaylistSearchQuery Map(PlaylistsSearchParameters model) =>
        new(model.Query, model.SortBy, model.Page, model.InverseOrder);

    public PlaylistViewModel Map(PlaylistDto playlist) => new(playlist.Id, playlist.Name, playlist.Genres,
        playlist.Description, playlist.PosterUri, playlist.Updated);

    public FilmViewModel Map(FilmDto film) => new(film.Id, film.Name, film.PosterUri, film.Rating,
        film.ShortDescription, film.Year, film.Type, film.CountSeasons, film.Genres);
}