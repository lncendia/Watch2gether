namespace Films.Infrastructure.Web.Rooms.ViewModels;

public class FilmRoomViewModel : FilmRoomShortViewModel
{
    public required int UserRatingsCount { get; init; }
    public required double? UserScore { get; init; }
    
    public required bool IsCodeNeeded { get; init; }
}