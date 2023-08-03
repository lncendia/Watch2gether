using Overoom.Application.Abstractions.MovieApi.DTOs;

namespace Overoom.Infrastructure.Movie.Abstractions;

public interface IBazonResponseParser
{
    public Cdn Get(string json);
}