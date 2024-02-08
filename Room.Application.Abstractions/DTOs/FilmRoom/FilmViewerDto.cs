using Room.Application.Abstractions.DTOs.Base;

namespace Room.Application.Abstractions.DTOs.FilmRoom;

public class FilmViewerDto : ViewerDto
{
    public int? Season { get; init; }
    public int? Series { get; init; }
}