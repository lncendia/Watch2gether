using Room.Application.Abstractions.DTOs.Rooms;

namespace Room.Application.Abstractions.DTOs.FilmRooms;

public class FilmViewerDto : ViewerDto
{
    public int? Season { get; init; }
    public int? Series { get; init; }
}