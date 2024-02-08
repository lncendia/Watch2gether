using Room.Domain.Users.Entities;
using Viewer = Room.Domain.Rooms.BaseRoom.Entities.Viewer;

namespace Room.Domain.Rooms.FilmRoom.Entities;

public class FilmViewer : Viewer
{
    internal FilmViewer(User user, int? season, int? series) : base(user)
    {
        Season = season;
        Series = series;
    }

    public int? Season { get; internal set; }
    public int? Series { get; internal set; }
}