using Room.Application.Abstractions.DTOs.Rooms;
using Room.Domain.Rooms.FilmRooms.ValueObjects;

namespace Room.Application.Abstractions.DTOs.FilmRooms;

public class FilmRoomDto : RoomDto<FilmViewerDto>
{
    public required string Title { get; init; }
    public required Cdn Cdn { get; init; }
    public required bool IsSerial { get; init; }
}