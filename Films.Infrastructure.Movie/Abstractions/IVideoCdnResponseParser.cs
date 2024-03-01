using Films.Application.Abstractions.MovieApi.DTOs;

namespace Films.Infrastructure.Movie.Abstractions;

public interface IVideoCdnResponseParser
{
    public CdnApiResponse Get(string json);
}