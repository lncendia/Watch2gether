using Room.Domain.Rooms.Rooms.Entities;

namespace Room.Domain.Rooms.FilmRooms.Entities;

public class FilmViewer : Viewer
{
    public int? Season { get; internal set; }
    public int? Series { get; internal set; }
}