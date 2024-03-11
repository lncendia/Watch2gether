using Films.Application.Abstractions.DTOs.Films;
using MediatR;

namespace Films.Application.Abstractions.Queries.Films;

public class UserHistoryQuery : IRequest<IReadOnlyCollection<FilmShortDto>>
{
    public required Guid Id { get; init; }
}