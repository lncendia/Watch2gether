using Room.Domain.Abstractions;
using Room.Domain.BaseRoom.Entities;
using Room.Domain.BaseRoom.Exceptions;
using Room.Domain.BaseRoom.ValueObjects;

namespace Room.Domain.BaseRoom;

public abstract class Room<T> : AggregateRoot where T : Viewer
{
    public DateTime LastActivity { get; private set; } = DateTime.UtcNow;

    public required T Owner
    {
        get => _owner;
        init
        {
            _owner = value;
            _viewersList.Add(value);
        }
    }

    private readonly List<T> _viewersList = [];

    private readonly List<Message> _messagesList = [];

    private readonly T _owner = null!;

    public IReadOnlyCollection<Message> Messages => _messagesList.OrderBy(m => m.CreatedAt).ToArray();

    public IReadOnlyCollection<T> Viewers => _viewersList;

    public void SetFullScreen(Guid targetId, bool isFullScreen)
    {
        UpdateActivity();
        var viewer = GetViewer(targetId);
        viewer.FullScreen = isFullScreen;
    }

    public void SetOnline(Guid targetId, bool isOnline)
    {
        UpdateActivity();
        var viewer = GetViewer(targetId);
        viewer.Online = isOnline;
        if (isOnline) return;
        viewer.Pause = true;
        viewer.FullScreen = false;
    }

    public void SetPause(Guid targetId, bool pause)
    {
        UpdateActivity();
        var viewer = GetViewer(targetId);
        viewer.Pause = pause;
    }

    public void SetTimeLine(Guid targetId, TimeSpan time)
    {
        UpdateActivity();
        var viewer = GetViewer(targetId);
        viewer.TimeLine = time;
    }

    public void SendMessage(Message message)
    {
        UpdateActivity();
        if (_messagesList.Count >= 100) _messagesList.RemoveAt(0);
        GetViewer(message.ViewerId);
        _messagesList.Add(message);
    }


    public void Beep(Guid initiatorId, Guid targetId)
    {
        UpdateActivity();

        var initiator = GetViewer(initiatorId);
        var target = GetViewer(targetId);

        if (initiatorId == targetId) throw new ActionNotAllowedException();
        if (!initiator.Allows.Beep) throw new ActionNotAllowedException();
        if (!target.Allows.Beep) throw new ActionNotAllowedException();
    }

    public void Scream(Guid initiatorId, Guid targetId)
    {
        UpdateActivity();
        var initiator = GetViewer(initiatorId);
        var target = GetViewer(targetId);
        if (initiatorId == targetId) throw new ActionNotAllowedException();
        if (!initiator.Allows.Scream) throw new ActionNotAllowedException();
        if (!target.Allows.Scream) throw new ActionNotAllowedException();
    }

    public virtual void Kick(Guid initiatorId, Guid targetId)
    {
        UpdateActivity();
        if (initiatorId == targetId || initiatorId != Owner.Id) throw new ActionNotAllowedException();
        var target = GetViewer(targetId);
        _viewersList.Remove(target);
    }

    public void ChangeName(Guid initiatorId, Guid targetId, string name)
    {
        UpdateActivity();

        var initiator = GetViewer(initiatorId);
        var target = GetViewer(targetId);

        if (initiatorId == targetId) throw new ActionNotAllowedException();
        if (!initiator.Allows.Change) throw new ActionNotAllowedException();
        if (!target.Allows.Change) throw new ActionNotAllowedException();
        target.Nickname = name;
    }

    protected void UpdateActivity() => LastActivity = DateTime.UtcNow;

    protected T GetViewer(Guid targetId)
    {
        var viewer = _viewersList.FirstOrDefault(x => x.Id == targetId);
        if (viewer == null) throw new ViewerNotFoundException();
        return viewer;
    }

    public virtual void Connect(T viewer)
    {
        UpdateActivity();
        if (_viewersList.Any(x => x.Id == viewer.Id)) throw new ViewerAlreadyExistsException();
        _viewersList.Add(viewer);
    }
    
    public virtual void Disconnect(Guid targetId)
    {
        UpdateActivity();
        var viewer = GetViewer(targetId);
        _viewersList.Remove(viewer);
    }
}