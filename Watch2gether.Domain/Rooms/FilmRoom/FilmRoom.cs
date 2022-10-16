using Watch2gether.Domain.Rooms.BaseRoom.Exceptions;
using Watch2gether.Domain.Rooms.FilmRoom.Entities;

namespace Watch2gether.Domain.Rooms.FilmRoom;

public class FilmRoom : BaseRoom.BaseRoom
{
    public FilmRoom(Guid filmId, string name, string avatarFileName)
    {
        FilmId = filmId;
        Owner = new FilmViewer(name, Id, avatarFileName, 1, 1);
    }

    public Guid FilmId { get; }
    public FilmViewer Owner { get; }
    public List<FilmViewer> Viewers => ViewersList.Select(x => (FilmViewer) x).ToList();


    public FilmViewer Connect(string name, string avatarFileName)
    {
        if (ViewersList.Count >= 10) throw new RoomIsFullException();

        var viewer = new FilmViewer(name, Id, avatarFileName, Owner.Season, Owner.Series);
        ViewersList.Add(viewer);
        return viewer;
    }

    public void ChangeSeason(Guid viewerId, int season)
    {
        var viewer = (FilmViewer) GetViewer(viewerId);
        viewer.Season = season;
        UpdateActivity();
    }

    public void ChangeSeries(Guid viewerId, int series)
    {
        var viewer = (FilmViewer) GetViewer(viewerId);
        viewer.Series = series;
        UpdateActivity();
    }
}