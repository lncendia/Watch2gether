using Overoom.Domain.Room.BaseRoom.Entities;

namespace Overoom.Domain.Room.FilmRoom.Entities;

public class FilmViewer : Viewer
{
    public FilmViewer(int id, string name, string avatarFileName, int season, int series) : base(id, name,
        avatarFileName)
    {
        Season = season;
        Series = series;
    }

    public int Season { get; set; }
    public int Series { get; set; }
}