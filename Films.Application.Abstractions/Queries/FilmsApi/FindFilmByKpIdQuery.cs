using Films.Application.Abstractions.Queries.FilmsApi.DTOs;
using MediatR;

namespace Films.Application.Abstractions.Queries.FilmsApi;

public class FindFilmByKpIdQuery : IRequest<FilmApiDto>
{
    public required long Id { get; init; }
}