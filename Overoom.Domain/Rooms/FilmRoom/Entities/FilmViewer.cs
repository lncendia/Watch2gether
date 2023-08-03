using Overoom.Domain.Rooms.BaseRoom.DTOs;
using Overoom.Domain.Rooms.BaseRoom.Entities;

namespace Overoom.Domain.Rooms.FilmRoom.Entities;

public class FilmViewer : Viewer
{
    internal FilmViewer(ViewerDto viewer, int id, int season, int series) : base(id, viewer)
    {
        Season = season;
        Series = series;
    }


    public int Season { get; set; }
    public int Series { get; set; }
}