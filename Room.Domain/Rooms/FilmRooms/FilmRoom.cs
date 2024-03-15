using Room.Domain.Rooms.FilmRooms.Entities;
using Room.Domain.Rooms.FilmRooms.Events;
using Room.Domain.Rooms.FilmRooms.Exceptions;
using Room.Domain.Rooms.FilmRooms.ValueObjects;
using Room.Domain.Rooms.Rooms;

namespace Room.Domain.Rooms.FilmRooms;

public class FilmRoom : Room<FilmViewer>
{
    /// <summary> 
    /// Заголовок фильма. 
    /// </summary> 
    private readonly string _title = null!;

    /// <summary> 
    /// Заголовок фильма. 
    /// </summary> 
    public required string Title
    {
        get => _title;
        init
        {
            if (string.IsNullOrEmpty(value) || value.Length > 200) throw new FilmTitleLengthException();
            _title = value;
        }
    }

    public required Cdn Cdn { get; init; }

    public required bool IsSerial { get; init; }

    public override void Connect(FilmViewer viewer)
    {
        viewer.Season = Owner.Season;
        viewer.Series = Owner.Series;
        base.Connect(viewer);
    }
    
    public override void Kick(Guid initiatorId, Guid targetId)
    {
        base.Kick(initiatorId, targetId);
        AddDomainEvent(new FilmRoomViewerKickedDomainEvent(this, targetId));
    }

    public void ChangeSeries(Guid target, int season, int series)
    {
        if (season < 1) throw new ArgumentOutOfRangeException(nameof(season), "Season must be positive");
        if (series < 1) throw new ArgumentOutOfRangeException(nameof(series), "Series must be positive");
        if (!IsSerial) throw new ChangeFilmSeriesException();
        var viewer = GetViewer(target);
        viewer.Season = season;
        viewer.Series = series;
    }
}