using Room.Application.Abstractions.DTOs.Base;
using Room.Domain.Films.Enums;

namespace Room.Application.Abstractions.DTOs.FilmRoom;

public class FilmRoomDto : RoomDto<FilmViewerDto>
{
    public required Guid FilmId { get; init; }
    public required string FilmTitle { get; init; }
    public required string FilmDescription { get; init; }
    public required Uri FilmUrl { get; init; }
    public required FilmType FilmType { get; init; }
}