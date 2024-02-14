using MediatR;
using Room.Domain.Films.Enums;
using Room.Domain.Films.ValueObjects;

namespace Room.Application.Abstractions.Commands.Films;

/// <summary>
/// Команда на добавление фильма
/// </summary>
public class AddFilmCommand : IRequest
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required FilmType Type { get; init; }
    public required Uri PosterUrl { get; init; }
    public required int Year { get; init; }
    public required IReadOnlyCollection<Cdn> CdnList { get; init; }
}