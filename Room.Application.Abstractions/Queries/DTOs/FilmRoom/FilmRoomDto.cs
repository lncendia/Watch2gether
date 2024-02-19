using Room.Application.Abstractions.Queries.DTOs.BaseRoom;

namespace Room.Application.Abstractions.Queries.DTOs.FilmRoom;

public class FilmRoomDto : RoomDto<FilmViewerDto>
{
    public required string Title { get; init; }
    public required Uri CdnUrl { get; init; }
    public required bool IsSerial { get; init; }
}