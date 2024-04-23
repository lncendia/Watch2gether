namespace Films.Infrastructure.Web.Rooms.ViewModels;

public class RoomsViewModel<T> where T : RoomViewModel
{
    public required IEnumerable<T> Rooms { get; init; }
    public required int CountPages { get; init; }
}