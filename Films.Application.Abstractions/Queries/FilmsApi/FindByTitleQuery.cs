using Films.Application.Abstractions.Queries.FilmsApi.DTOs;
using MediatR;

namespace Films.Application.Abstractions.Queries.FilmsApi;

public class FindByTitleQuery : IRequest<FilmApiDto>
{
    public required string Title { get; init; }
}