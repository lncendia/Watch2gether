using Overoom.Application.Abstractions.Content.DTOs;
using Overoom.WEB.Contracts.Content;
using Overoom.WEB.Models.Content;

namespace Overoom.WEB.Mappers.Abstractions;

public interface IContentMapper
{
    public FilmSearchQuery Map(FilmsSearchParameters model);
    public PlaylistSearchQuery Map(PlaylistsSearchParameters model);
    public PlaylistViewModel Map(PlaylistDto playlist);
    public FilmViewModel Map(FilmDto film);
}