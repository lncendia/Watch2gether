using Overoom.Domain.Film.Enums;
using Overoom.Domain.Room.BaseRoom.Exceptions;
using Overoom.Domain.Room.BaseRoom.ValueObject;

namespace Overoom.Domain.Room.FilmRoom.Entities;

public class FilmRoom : BaseRoom.Entities.BaseRoom
{
    public FilmRoom(Guid filmId, string name, string avatarFileName, CdnType cdnType)
    {
        FilmId = filmId;
        CdnType = cdnType;
        Owner = new FilmViewer(_idCounter, name, avatarFileName, 1, 1);
        ViewersList.Add(Owner);
    }

    public Guid FilmId { get; }
    public CdnType CdnType { get; }
    public FilmViewer Owner { get; }
    public IReadOnlyCollection<FilmViewer> Viewers => ViewersList.Cast<FilmViewer>().ToList().AsReadOnly();
    public IReadOnlyCollection<Message> Messages => MessagesList.AsReadOnly();
    private int _idCounter = 1;


    public FilmViewer Connect(string name, string avatarFileName)
    {
        if (ViewersList.Count >= 10) throw new RoomIsFullException();
        _idCounter++;
        var viewer = new FilmViewer(_idCounter, name, avatarFileName, Owner.Season, Owner.Series);
        AddViewer(viewer);
        return viewer;
    }

    public void SendMessage(int viewerId, string message)
    {
        SetOnline(viewerId, true);
        var messageV = new Message(viewerId, message, Id);
        AddMessage(messageV);
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