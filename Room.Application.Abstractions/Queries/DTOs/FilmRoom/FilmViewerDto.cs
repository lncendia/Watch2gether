using Room.Application.Abstractions.Queries.DTOs.BaseRoom;

namespace Room.Application.Abstractions.Queries.DTOs.FilmRoom;

public class FilmViewerDto : ViewerDto
{
    public int? Season { get; init; }
    public int? Series { get; init; }
}