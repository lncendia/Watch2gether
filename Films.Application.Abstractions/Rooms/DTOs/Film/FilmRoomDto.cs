namespace Films.Application.Abstractions.Rooms.DTOs.Film;

public class FilmRoomDto : RoomDto
{
    public required FilmDataDto Film { get; init; }
    public required IReadOnlyCollection<FilmViewerDto> Viewers { get; init; }
}