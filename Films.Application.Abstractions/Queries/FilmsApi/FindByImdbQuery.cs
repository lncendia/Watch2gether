using Films.Application.Abstractions.Queries.FilmsApi.DTOs;
using MediatR;

namespace Films.Application.Abstractions.Queries.FilmsApi;

public class FindByImdbQuery : IRequest<FilmApiDto>
{
    public required string Imdb { get; init; }
}