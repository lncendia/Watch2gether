using Overoom.Application.Abstractions.Content.DTOs;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Playlists.Entities;

namespace Overoom.Application.Abstractions.Content.Interfaces;

public interface IContentMapper
{
    FilmDto Map(Film film);
    PlaylistDto Map(Playlist playlist);
}