using Films.Application.Abstractions.DTOs.Films;
using MediatR;

namespace Films.Application.Abstractions.Queries.Films;

public class PopularFilmsQuery : IRequest<IReadOnlyCollection<FilmShortDto>>
{
    public required int Take { get; init; }
}