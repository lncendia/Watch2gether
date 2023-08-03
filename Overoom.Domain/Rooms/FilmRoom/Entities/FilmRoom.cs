using Overoom.Domain.Films.Enums;
using Overoom.Domain.Rooms.BaseRoom.DTOs;
using Overoom.Domain.Rooms.BaseRoom.Entities;

namespace Overoom.Domain.Rooms.FilmRoom.Entities;

public class FilmRoom : Room
{
    public FilmRoom(Guid filmId, CdnType cdnType, bool isOpen, ViewerDto viewer) : base(isOpen, CreateViewer(viewer))
    {
        FilmId = filmId;
        CdnType = cdnType;
    }

    private static Viewer CreateViewer(ViewerDto viewer) => new FilmViewer(viewer, 1, 1, 1);

    public Guid FilmId { get; }
    public CdnType CdnType { get; }
    public new FilmViewer Owner => (FilmViewer)base.Owner;
    public new IReadOnlyCollection<FilmViewer> Viewers => base.Viewers.Cast<FilmViewer>().ToList();

    public int Connect(ViewerDto viewer)
    {
        var filmViewer = new FilmViewer(viewer, GetNextId(), Owner.Season, Owner.Series);
        return AddViewer(filmViewer);
    }

    public void ChangeSeason(int viewerId, int season)
    {
        var viewer = (FilmViewer)GetViewer(viewerId);
        viewer.Season = season;
        UpdateActivity();
    }

    public void ChangeSeries(int viewerId, int series)
    {
        var viewer = (FilmViewer)GetViewer(viewerId);
        viewer.Series = series;
        UpdateActivity();
    }
}