using Overoom.Application.Abstractions.Movie.DTOs;
using Overoom.Domain.Films.Enums;

namespace Overoom.Application.Abstractions.Movie.Interfaces;

public interface IFilmManager
{
    Task<FilmDto> GetAsync(Guid id, Guid? userId);
    Task ToggleWatchlistAsync(Guid id, Guid userId);
    Task<RatingDto> AddRatingAsync(Guid id, Guid userId, double score);
    Task<Uri> GetFilmUriAsync(Guid id, CdnType type);

    /// <summary>
    /// Асинхронно добавляет фильм в историю.
    /// </summary>
    /// <param name="id">Идентификатор фильма.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    public Task AddToHistoryAsync(Guid id, Guid userId);
}