using Overoom.Application.Abstractions.Kinopoisk.DTOs;

namespace Overoom.Infrastructure.Movie.Abstractions;

public interface IVideoCdnResponseParser
{
    public Cdn Get(string json);
}