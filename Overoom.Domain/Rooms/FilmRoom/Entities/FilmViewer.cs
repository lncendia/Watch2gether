using Overoom.Domain.Rooms.BaseRoom.Entities;

namespace Overoom.Domain.Rooms.FilmRoom.Entities;

public class FilmViewer : Viewer
{
    public FilmViewer(string name, Guid roomId, string avatarFileName, int season, int series) : base(name, roomId,
        avatarFileName)
    {
        Season = season;
        Series = series;
    }

    public int Season { get; set; }
    public int Series { get; set; }
}