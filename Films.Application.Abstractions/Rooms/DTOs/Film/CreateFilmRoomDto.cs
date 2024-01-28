using Films.Domain.Films.Enums;

namespace Films.Application.Abstractions.Rooms.DTOs.Film;

public class CreateFilmRoomDto : CreateRoomDto
{
    public required Guid FilmId { get; init; }
    public required CdnType CdnType { get; init; }
}