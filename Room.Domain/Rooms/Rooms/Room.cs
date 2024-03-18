using Room.Domain.Abstractions;
using Room.Domain.Rooms.Rooms.Entities;
using Room.Domain.Rooms.Rooms.Exceptions;

namespace Room.Domain.Rooms.Rooms;

public abstract class Room<T> : AggregateRoot where T : Viewer
{
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

    private readonly T _owner = null!;

    public IReadOnlyCollection<T> Viewers => _viewersList;

    public void SetFullScreen(Guid targetId, bool isFullScreen)
    {
        var viewer = GetViewer(targetId);
        viewer.FullScreen = isFullScreen;
    }

    public void SetOnline(Guid targetId, bool isOnline)
    {
        var viewer = GetViewer(targetId);
        viewer.Online = isOnline;
        if (isOnline) return;
        viewer.Pause = true;
        viewer.FullScreen = false;
    }

    public void SetPause(Guid targetId, bool pause)
    {
        var viewer = GetViewer(targetId);
        viewer.Pause = pause;
    }

    public void SetTimeLine(Guid targetId, TimeSpan time)
    {
        var viewer = GetViewer(targetId);
        viewer.TimeLine = time;
    }


    public void Beep(Guid initiatorId, Guid targetId)
    {
        var initiator = GetViewer(initiatorId);
        var target = GetViewer(targetId);

        if (initiatorId == targetId) throw new ActionNotAllowedException();
        if (!initiator.Allows.Beep) throw new ActionNotAllowedException();
        if (!target.Allows.Beep) throw new ActionNotAllowedException();
    }

    public void Scream(Guid initiatorId, Guid targetId)
    {
        var initiator = GetViewer(initiatorId);
        var target = GetViewer(targetId);
        if (initiatorId == targetId) throw new ActionNotAllowedException();
        if (!initiator.Allows.Scream) throw new ActionNotAllowedException();
        if (!target.Allows.Scream) throw new ActionNotAllowedException();
    }

    public virtual void Kick(Guid initiatorId, Guid targetId)
    {
        if (initiatorId == targetId || initiatorId != Owner.Id) throw new ActionNotAllowedException();
        var target = GetViewer(targetId);
        _viewersList.Remove(target);
    }

    public void ChangeName(Guid initiatorId, Guid targetId, string name)
    {
        var initiator = GetViewer(initiatorId);
        var target = GetViewer(targetId);
        
        if (!initiator.Allows.Change) throw new ActionNotAllowedException();
        if (!target.Allows.Change) throw new ActionNotAllowedException();
        target.Username = name;
    }

    protected T GetViewer(Guid targetId)
    {
        var viewer = _viewersList.FirstOrDefault(x => x.Id == targetId);
        if (viewer == null) throw new ViewerNotFoundException();
        return viewer;
    }

    public virtual void Connect(T viewer)
    {
        if (_viewersList.Any(x => x.Id == viewer.Id)) throw new ViewerAlreadyExistsException();
        _viewersList.Add(viewer);
    }
    
    public void Disconnect(Guid targetId)
    {
        var viewer = GetViewer(targetId);
        _viewersList.Remove(viewer);
    }
}