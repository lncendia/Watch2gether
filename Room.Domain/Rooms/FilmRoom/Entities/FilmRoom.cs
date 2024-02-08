using Room.Domain.Films.Entities;
using Room.Domain.Films.Enums;
using Room.Domain.Rooms.BaseRoom.Entities;
using Room.Domain.Rooms.FilmRoom.Exceptions;
using Room.Domain.Users.Entities;

namespace Room.Domain.Rooms.FilmRoom.Entities;

public class FilmRoom(User user, Film film, string cdnName, bool isOpen)
    : Room<FilmViewer>(new FilmViewer(user, 1, 1), isOpen)
{
    public Guid FilmId { get; } = film.Id;
    public string CdnName { get; } = cdnName;

    public override void Connect(User user, string? code = null)
    {
        var filmViewer = new FilmViewer(user, Owner.Season, Owner.Series);
        AddViewer(filmViewer, code);
    }

    public void ChangeSeries(Film film, Guid target, int season, int series)
    {
        if (season < 1) throw new ArgumentOutOfRangeException(nameof(season), "Season must be positive");
        if (series < 1) throw new ArgumentOutOfRangeException(nameof(series), "Series must be positive");
        if (FilmId != film.Id) throw new ArgumentException("The film is wrong", nameof(film));
        if (film.Type == FilmType.Film) throw new ChangeFilmSeriesException();
        UpdateActivity();
        var viewer = GetViewer(target);
        viewer.Season = season;
        viewer.Series = series;
    }
}