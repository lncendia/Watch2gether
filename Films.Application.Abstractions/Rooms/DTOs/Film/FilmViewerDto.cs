namespace Films.Application.Abstractions.Rooms.DTOs.Film;

public class FilmViewerDto : ViewerDto
{
    public required int Season { get; init; }
    public required int Series { get; init; }
}