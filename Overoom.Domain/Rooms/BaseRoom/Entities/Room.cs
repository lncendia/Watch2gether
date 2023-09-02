using Overoom.Domain.Abstractions;
using Overoom.Domain.Rooms.BaseRoom.Exceptions;
using Overoom.Domain.Rooms.BaseRoom.ValueObjects;

namespace Overoom.Domain.Rooms.BaseRoom.Entities;

public abstract class Room : AggregateRoot
{
    protected Room(bool isOpen, Viewer owner)
    {
        AddViewer(owner);
        IsOpen = isOpen;
        Owner = owner;
    }

    public bool IsOpen { get; }
    public DateTime LastActivity { get; private set; } = DateTime.UtcNow;

    private readonly List<Viewer> _viewersList = new();

    private readonly List<Message> _messagesList = new();
    protected readonly Viewer Owner;
    private int _idCounter = 1;

    public IReadOnlyCollection<Message> Messages => _messagesList.AsReadOnly();
    protected IReadOnlyCollection<Viewer> Viewers => _viewersList.AsReadOnly();

    public void SetFullScreen(int viewerId, bool isFullScreen)
    {
        var viewer = GetViewer(viewerId);
        viewer.FullScreen = isFullScreen;
        UpdateActivity();
    }

    public void SetOnline(int viewerId, bool isOnline)
    {
        var viewer = GetViewer(viewerId);
        viewer.Online = isOnline;
        if (!isOnline)
        {
            viewer.Pause = true;
            viewer.FullScreen = false;
        }

        UpdateActivity();
    }

    public void SetPause(int viewerId, bool pause)
    {
        var viewer = GetViewer(viewerId);
        viewer.Pause = pause;
        UpdateActivity();
    }

    public void SetTimeLine(int viewerId, TimeSpan time)
    {
        var viewer = GetViewer(viewerId);
        viewer.TimeLine = time;
        UpdateActivity();
    }

    public void SendMessage(int viewerId, string message)
    {
        if (_messagesList.Count >= 100) _messagesList.RemoveAt(0);
        SetOnline(viewerId, true);
        var messageV = new Message(viewerId, message);
        _messagesList.Add(messageV);
        UpdateActivity();
    }


    public void Beep(int viewerId, int target)
    {
        UpdateActivity();
        if (viewerId == target) throw new ActionNotAllowedException();
        var viewer = GetViewer(target);
        if (!viewer.Allows.Beep) throw new ActionNotAllowedException();
    }

    public void Scream(int viewerId, int target)
    {
        UpdateActivity();
        if (viewerId == target) throw new ActionNotAllowedException();
        var viewer = GetViewer(target);
        if (!viewer.Allows.Scream) throw new ActionNotAllowedException();
    }

    public void Kick(int viewerId, int target)
    {
        UpdateActivity();
        if (viewerId == target || viewerId != Owner.Id) throw new ActionNotAllowedException();
        var targetViewer = GetViewer(target);
        _viewersList.Remove(targetViewer);
        _messagesList.RemoveAll(x => x.ViewerId == target);
    }

    public void ChangeName(int viewerId, int target, string name)
    {
        UpdateActivity();
        if (viewerId == target) throw new ActionNotAllowedException();
        var viewer = GetViewer(target);
        if (!viewer.Allows.Change) throw new ActionNotAllowedException();
        viewer.Name = name;
    }

    protected void UpdateActivity() => LastActivity = DateTime.UtcNow;

    protected Viewer GetViewer(int viewerId)
    {
        var viewer = _viewersList.FirstOrDefault(x => x.Id == viewerId);
        if (viewer == null) throw new ViewerNotFoundException();
        return viewer;
    }

    protected int GetNextId() => _idCounter;

    protected int AddViewer(Viewer viewer)
    {
        if (_viewersList.Any(x => x.Id == viewer.Id)) throw new ViewerAlreadyExistsException();
        viewer.Online = true;
        _viewersList.Add(viewer);
        UpdateActivity();
        _idCounter++;
        return viewer.Id;
    }
}