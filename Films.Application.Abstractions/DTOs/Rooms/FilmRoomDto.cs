namespace Films.Application.Abstractions.DTOs.Rooms;

public class FilmRoomDto : FilmRoomShortDto
{
    public required int UserRatingsCount { get; init; }
    public double? UserScore { get; init; }
    public required bool IsCodeNeeded { get; init; }
}