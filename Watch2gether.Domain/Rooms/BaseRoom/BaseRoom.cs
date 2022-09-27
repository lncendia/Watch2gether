using Watch2gether.Domain.Rooms.BaseRoom.Entities;
using Watch2gether.Domain.Rooms.BaseRoom.Exceptions;
using Watch2gether.Domain.Rooms.BaseRoom.ValueObject;

namespace Watch2gether.Domain.Rooms.BaseRoom;

public abstract class BaseRoom
{
    protected BaseRoom(string name, string avatarFileName)
    {
        Id = Guid.NewGuid();
        Owner = Connect(name, avatarFileName);
    }

    public Guid Id { get; }
    public bool IsOpen { get; set; } = false;
    protected readonly List<Viewer> _viewers = new();
    protected readonly List<Message> _messages = new();

    public List<Viewer> Viewers => _viewers.ToList();
    public List<Message> Messages => _messages.ToList();
    public Viewer Owner { get; }
    public DateTime LastActivity { get; private set; } = DateTime.UtcNow;

    public Viewer Connect(string name, string avatarFileName)
    {
        var viewer = new Viewer(name, Id, avatarFileName);
        _viewers.Add(viewer);
        UpdateActivity();
        return viewer;
    }

    public void SendMessage(Guid viewerId, string message)
    {
        SetOnline(viewerId, true);
        _messages.Add(new Message(viewerId, message, Id));
        UpdateActivity();
    }

    public void UpdateViewer(Guid viewerId, bool pause, TimeSpan time)
    {
        var viewer = _viewers.FirstOrDefault(x => x.Id == viewerId);
        if (viewer == null) throw new ViewerNotFoundException();
        viewer.OnPause = pause;
        viewer.TimeLine = time;
        UpdateActivity();
    }

    public void SetOnline(Guid viewerId, bool isOnline)
    {
        var viewer = _viewers.FirstOrDefault(x => x.Id == viewerId);
        if (viewer == null) throw new ViewerNotFoundException();
        viewer.Online = isOnline;
        UpdateActivity();
    }

    protected void UpdateActivity() => LastActivity = DateTime.UtcNow;
}