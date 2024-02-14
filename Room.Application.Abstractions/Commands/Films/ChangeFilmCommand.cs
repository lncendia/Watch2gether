using MediatR;
using Room.Domain.Films.ValueObjects;

namespace Room.Application.Abstractions.Commands.Films;

/// <summary>
/// Команда на изменение фильма
/// </summary>
public class ChangeFilmCommand : IRequest
{
    public required Guid Id { get; init; }
    public required string Description { get; init; }
    public required Uri PosterUrl { get; init; }
    public required IReadOnlyCollection<Cdn> CdnList { get; init; }
}