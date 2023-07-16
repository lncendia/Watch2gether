using Overoom.Application.Abstractions.Movie.DTOs;
using Overoom.Domain.Films.Enums;

namespace Overoom.Application.Abstractions.Movie.Interfaces;

public interface IFilmManager
{
    Task<FilmDto> GetAsync(Guid id, Guid? userId);
    Task ToggleWatchlistAsync(Guid id, Guid userId);
    Task<RatingDto> AddRatingAsync(Guid id, Guid userId, double score);
    Task<Uri> GetFilmUriAsync(Guid id, CdnType type);
}