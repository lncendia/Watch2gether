using Viewer = Room.Domain.BaseRoom.Entities.Viewer;

namespace Room.Domain.FilmRooms.Entities;

public class FilmViewer : Viewer
{
    public int? Season { get; internal set; }
    public int? Series { get; internal set; }
}