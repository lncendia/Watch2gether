using Overoom.Domain.Abstractions;
using Overoom.Domain.Rooms.BaseRoom.Exceptions;
using Overoom.Domain.Rooms.BaseRoom.ValueObject;

namespace Overoom.Domain.Rooms.BaseRoom.Entities;

public abstract class Room : AggregateRoot
{
    public bool IsOpen { get; set; } = false;
    public DateTime LastActivity { get; private set; } = DateTime.UtcNow;

    protected readonly List<Viewer> ViewersList = new();

    private readonly List<Message> _messagesList = new();
    protected Viewer? Owner = null;
    protected int IdCounter = 1;

    public IReadOnlyCollection<Message> Messages => _messagesList.AsReadOnly();

    public virtual void SetOnline(int viewerId, bool isOnline)
    {
        var viewer = GetViewer(viewerId);
        viewer.Online = isOnline;
        if (!isOnline) viewer.OnPause = true;
        UpdateActivity();
    }

    public virtual void UpdateTimeLine(int viewerId, bool pause, TimeSpan time)
    {
        var viewer = GetViewer(viewerId);
        viewer.OnPause = pause;
        viewer.TimeLine = time;
        UpdateActivity();
    }

    protected void UpdateActivity() => LastActivity = DateTime.UtcNow;

    protected Viewer GetViewer(int viewerId)
    {
        var viewer = ViewersList.FirstOrDefault(x => x.Id == viewerId);
        if (viewer == null) throw new ViewerNotFoundException();
        return viewer;
    }

    public virtual void SendMessage(int viewerId, string message)
    {
        if (_messagesList.Count >= 100) _messagesList.RemoveAt(0);
        SetOnline(viewerId, true);
        var messageV = new Message(viewerId, message);
        _messagesList.Add(messageV);
        UpdateActivity();
    }

    protected void AddViewer(Viewer viewer)
    {
        if (ViewersList.Any(x => x.Id == viewer.Id)) throw new ViewerAlreadyExistsException();
        ViewersList.Add(viewer);
        UpdateActivity();
        IdCounter++;
    }
}