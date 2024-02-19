namespace Films.Infrastructure.Web.Rooms.ViewModels;

public class FilmRoomViewModel : RoomViewModel
{
    public required string Title { get; init; }
    public required string PosterUrl { get; init; }
    public required int Year { get; init; }
    public required double UserRating { get; init; }

    public double? RatingKp { get; init; }
    public double? RatingImdb { get; init; }
    public required string Description { get; init; }
    public required bool IsSerial { get; init; }
}