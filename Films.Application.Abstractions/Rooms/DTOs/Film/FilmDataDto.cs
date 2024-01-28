using Films.Domain.Films.Enums;

namespace Films.Application.Abstractions.Rooms.DTOs.Film;

public class FilmDataDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required Uri Url { get; init; }
    public required CdnType Cdn { get; init; }
    public required FilmType Type { get; init; }
}