using Overoom.Domain.Rooms.BaseRoom.Entities;

namespace Overoom.Domain.Rooms.FilmRoom.Entities;

public class FilmViewer : Viewer
{
    internal FilmViewer(int id, string name, Uri avatarUri, int season, int series) : base(id, name,
        avatarUri)
    {
        Season = season;
        Series = series;
    }

    public int Season { get; set; }
    public int Series { get; set; }
}