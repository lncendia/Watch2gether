using Overoom.Domain.Rooms.BaseRoom.Exceptions;
using Overoom.Domain.Rooms.BaseRoom.ValueObject;
using Overoom.Domain.Rooms.FilmRoom.Entities;

namespace Overoom.Domain.Rooms.FilmRoom;

public class FilmRoom : BaseRoom.BaseRoom
{
    public FilmRoom(Guid filmId, string name, string avatarFileName)
    {
        FilmId = filmId;
        Owner = new FilmViewer(name, Id, avatarFileName, 1, 1);
        ViewersList.Add(Owner);
    }

    public Guid FilmId { get; }
    public FilmViewer Owner { get; }
    public List<FilmViewer> Viewers => ViewersList.Cast<FilmViewer>().ToList();
    public List<Message> Messages => MessagesList.ToList();


    public FilmViewer Connect(string name, string avatarFileName)
    {
        if (ViewersList.Count >= 10) throw new RoomIsFullException();
        var viewer = new FilmViewer(name, Id, avatarFileName, Owner.Season, Owner.Series);
        AddViewer(viewer);
        return viewer;
    }

    public void SendMessage(Guid viewerId, string message)
    {
        SetOnline(viewerId, true);
        var messageV = new Message(viewerId, message, Id);
        AddMessage(messageV);
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