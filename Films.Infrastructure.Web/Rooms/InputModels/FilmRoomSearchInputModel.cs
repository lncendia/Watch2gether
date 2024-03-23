namespace Films.Infrastructure.Web.Rooms.InputModels;

public class FilmRoomSearchInputModel : RoomSearchInputModel
{
    public Guid? FilmId { get; init; }
}