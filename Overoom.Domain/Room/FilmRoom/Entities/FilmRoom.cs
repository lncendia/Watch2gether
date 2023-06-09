using Overoom.Domain.Film.Enums;
using Overoom.Domain.Room.BaseRoom.ValueObject;

namespace Overoom.Domain.Room.FilmRoom.Entities;

public class FilmRoom : BaseRoom.Entities.Room
{
    public FilmRoom(Guid filmId, string name, string avatarUri, CdnType cdnType)
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