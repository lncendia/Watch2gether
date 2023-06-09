using Overoom.Domain.Room.BaseRoom.Entities;

namespace Overoom.Domain.Room.FilmRoom.Entities;

public class FilmViewer : Viewer
{
    internal FilmViewer(int id, string name, string avatarUri, int season, int series) : base(id, name,
        avatarUri)
    {
        Season = season;
        Series = series;
    }

    public int Season { get; set; }
    public int Series { get; set; }
}