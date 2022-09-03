using Watch2gether.Domain.Rooms.Entities;
using Watch2gether.Domain.Rooms.Exceptions;
using Watch2gether.Domain.Rooms.ValueObject;

namespace Watch2gether.Domain.Rooms;

public class Room
{
    public Room(Guid filmId, string name, string avatarFileName)
    {
        Id = Guid.NewGuid();
        FilmId = filmId;
        Owner = Connect(name, avatarFileName);
    }

    public Guid Id { get; }
    public Guid FilmId { get; }
    private readonly List<Viewer> _viewers = new();
    private readonly List<Message> _messages = new();
    public List<Viewer> Viewers => _viewers.ToList();
    public List<Message> Messages => _messages.ToList();
    public Viewer Owner { get; }
    public DateTime LastActivity { get; private set; } = DateTime.UtcNow;

    public Viewer Connect(string name, string avatarFileName)
    {
        var viewer = new Viewer(name, Id, avatarFileName);
        _viewers.Add(viewer);
        LastActivity = DateTime.UtcNow;
        return viewer;
    }

    public void SendMessage(Guid viewerId, string message)
    {
        SetOnline(viewerId, true);
        _messages.Add(new Message(viewerId, message, Id));
        LastActivity = DateTime.UtcNow;
    }

    public void UpdateViewer(Guid viewerId, bool pause, TimeSpan time)
    {
        var viewer = _viewers.FirstOrDefault(x => x.Id == viewerId);
        if (viewer == null) throw new ViewerNotFoundException();
        viewer.OnPause = pause;
        viewer.TimeLine = time;
        LastActivity = DateTime.UtcNow;
    }

    public void SetOnline(Guid viewerId, bool isOnline)
    {
        var viewer = _viewers.FirstOrDefault(x => x.Id == viewerId);
        if (viewer == null) throw new ViewerNotFoundException();
        viewer.Online = isOnline;
        LastActivity = DateTime.UtcNow;
    }
}