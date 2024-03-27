namespace Films.Application.Abstractions.DTOs.Rooms;

public class FilmRoomDto : FilmRoomShortDto
{
    public required int UserRatingsCount { get; init; }
    public required double? UserScore { get; init; }
}