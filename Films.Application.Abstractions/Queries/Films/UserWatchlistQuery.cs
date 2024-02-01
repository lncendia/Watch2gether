using Films.Application.Abstractions.Queries.Films.DTOs;
using MediatR;

namespace Films.Application.Abstractions.Queries.Films;

public class UserWatchlistQuery : IRequest<IReadOnlyCollection<FilmShortDto>>
{
    public required Guid Id { get; init; }
}