using Overoom.Application.Abstractions.MovieApi.DTOs;

namespace Overoom.Infrastructure.Movie.Abstractions;

public interface IVideoCdnResponseParser
{
    public Cdn Get(string json);
}