using MediatR;
using Films.Application.Abstractions.Queries.Films.DTOs;

namespace Films.Application.Abstractions.Queries.Films;

public class PopularFilmsQuery : IRequest<IReadOnlyCollection<FilmShortDto>>
{
    public required int Take { get; init; }
}