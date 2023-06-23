using Overoom.Domain.Films.Enums;

namespace Overoom.Domain.Rooms.FilmRoom.Entities;

public class FilmRoom : BaseRoom.Entities.Room
{
    public FilmRoom(Guid filmId, string name, Uri avatarUri, CdnType cdnType)
    {
        FilmId = filmId;
        CdnType = cdnType;
        base.Owner = new FilmViewer(IdCounter, name, avatarUri, 1, 1);
        IdCounter++;
        ViewersList.Add(Owner);
    }

    public Guid FilmId { get; }
    public CdnType CdnType { get; }
    public new FilmViewer Owner => (FilmViewer)base.Owner!;
    public IReadOnlyCollection<FilmViewer> Viewers => ViewersList.Cast<FilmViewer>().ToList().AsReadOnly();

    public FilmViewer Connect(string name, Uri avatarFileName)
    {
        var viewer = new FilmViewer(IdCounter, name, avatarFileName, Owner.Season, Owner.Series);
        AddViewer(viewer);
        return viewer;
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