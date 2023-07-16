using Overoom.Application.Abstractions.Content.DTOs;

namespace Overoom.Application.Abstractions.Content.Interfaces;

public interface IContentManager
{
    Task<List<FilmDto>> FindAsync(FilmSearchQuery searchQuery);
    Task<List<PlaylistDto>> FindAsync(PlaylistSearchQuery playlistSearchQuery);
}