using Films.Application.Abstractions.Queries.Films.DTOs;
using MediatR;

namespace Films.Application.Abstractions.Queries.Films;

public class UserHistoryQuery : IRequest<IReadOnlyCollection<FilmShortDto>>
{
    public required Guid Id { get; init; }
}