using Films.Application.Abstractions.Services.MovieApi.DTOs;

namespace Films.Infrastructure.Movie.Abstractions;

public interface IBazonResponseParser
{
    public CdnApiResponse Get(string json);
}