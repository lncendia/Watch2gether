using Films.Application.Abstractions.MovieApi.DTOs;

namespace Films.Infrastructure.Movie.Abstractions;

public interface IBazonResponseParser
{
    public CdnApiResponse Get(string json);
}