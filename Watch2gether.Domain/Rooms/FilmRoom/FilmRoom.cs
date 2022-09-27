using Watch2gether.Domain.Rooms.BaseRoom.Exceptions;

namespace Watch2gether.Domain.Rooms.FilmRoom;

public class FilmRoom : BaseRoom.BaseRoom
{
    public FilmRoom(Guid filmId, string name, string avatarFileName) : base(name, avatarFileName) => FilmId = filmId;

    public Guid FilmId { get; }

    public void ChangeSeason(Guid viewerId, int season)
    {
        var viewer = _viewers.FirstOrDefault(x => x.Id == viewerId);
        if (viewer == null) throw new ViewerNotFoundException();
        viewer.Season = season;
        UpdateActivity();
    }

    public void ChangeSeries(Guid viewerId, int series)
    {
        var viewer = _viewers.FirstOrDefault(x => x.Id == viewerId);
        if (viewer == null) throw new ViewerNotFoundException();
        viewer.Series = series;
        UpdateActivity();
    }
}