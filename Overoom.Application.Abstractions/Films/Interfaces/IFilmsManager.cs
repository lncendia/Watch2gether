using Overoom.Application.Abstractions.Films.DTOs;

namespace Overoom.Application.Abstractions.Films.Interfaces;

public interface IFilmsManager
{
    Task<List<FilmDto>> PopularFilmsAsync();
    Task<List<FilmDto>> FindAsync(FilmSearchQuery searchQuery);
    Task<string> GetPlaylistNameAsync(Guid id);
}