using System.Text;
using Room.Domain.Abstractions;
using Room.Domain.Rooms.BaseRoom.Exceptions;
using Room.Domain.Rooms.BaseRoom.ValueObjects;
using Room.Domain.Users.Entities;

namespace Room.Domain.Rooms.BaseRoom.Entities;

public abstract class Room<T> : AggregateRoot where T : Viewer
{
    protected Room(T owner, bool isOpen) : base(Guid.NewGuid())
    {
        Owner = owner;
        if (isOpen) Code = GenerateRandomCode(5);
    }

    private static string GenerateRandomCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var sb = new StringBuilder();

        var random = new Random();
        for (var i = 0; i < length; i++)
        {
            var index = random.Next(chars.Length);
            sb.Append(chars[index]);
        }

        return sb.ToString();
    }

    public DateTime LastActivity { get; private set; } = DateTime.UtcNow;
    public string? Code { get; }
    public T Owner { get; }

    private readonly List<T> _viewersList = [];

    private readonly List<Message> _messagesList = [];

    private readonly List<Guid> _bannedList = [];

    public IReadOnlyCollection<Message> Messages => _messagesList.OrderBy(m=>m.CreatedAt).ToArray();

    public IReadOnlyCollection<T> Viewers => _viewersList;

    public void SetFullScreen(Guid target, bool isFullScreen)
    {
        UpdateActivity();
        var viewer = GetViewer(target);
        viewer.FullScreen = isFullScreen;
    }

    public void SetOnline(Guid target, bool isOnline)
    {
        UpdateActivity();
        var viewer = GetViewer(target);
        viewer.Online = isOnline;
        if (isOnline) return;
        viewer.Pause = true;
        viewer.FullScreen = false;
    }

    public void SetPause(Guid target, bool pause)
    {
        UpdateActivity();
        var viewer = GetViewer(target);
        viewer.Pause = pause;
    }

    public void SetTimeLine(Guid target, TimeSpan time)
    {
        UpdateActivity();
        var viewer = GetViewer(target);
        viewer.TimeLine = time;
    }

    public void SendMessage(Message message)
    {
        UpdateActivity();
        if (_messagesList.Count >= 100) _messagesList.RemoveAt(0);
        GetViewer(message.UserId);
        _messagesList.Add(message);
    }


    public void Beep(User initiator, User target)
    {
        UpdateActivity();
        GetViewer(initiator.Id);
        GetViewer(target.Id);
        if (initiator.Id == target.Id) throw new ActionNotAllowedException();
        if (!initiator.Allows.Beep) throw new ActionNotAllowedException();
        if (!target.Allows.Beep) throw new ActionNotAllowedException();
    }

    public void Scream(User initiator, User target)
    {
        UpdateActivity();
        GetViewer(initiator.Id);
        GetViewer(target.Id);
        if (initiator.Id == target.Id) throw new ActionNotAllowedException();
        if (!initiator.Allows.Scream) throw new ActionNotAllowedException();
        if (!target.Allows.Scream) throw new ActionNotAllowedException();
    }

    public void Kick(Guid initiator, Guid target)
    {
        UpdateActivity();
        if (initiator == target || initiator != Owner.UserId) throw new ActionNotAllowedException();
        var targetViewer = GetViewer(target);
        _viewersList.Remove(targetViewer);
        _bannedList.Add(target);
    }

    public void ChangeName(User initiator, User target, string name)
    {
        UpdateActivity();
        GetViewer(initiator.Id);
        var targetViewer = GetViewer(target.Id);
        if (initiator.Id == target.Id) throw new ActionNotAllowedException();
        if (!initiator.Allows.Change) throw new ActionNotAllowedException();
        if (!target.Allows.Change) throw new ActionNotAllowedException();
        targetViewer.CustomName = name;
    }

    protected void UpdateActivity() => LastActivity = DateTime.UtcNow;

    protected T GetViewer(Guid target)
    {
        var viewer = _viewersList.FirstOrDefault(x => x.UserId == target);
        if (viewer == null) throw new ViewerNotFoundException();
        return viewer;
    }

    public abstract void Connect(User user, string? code = null);

    protected void AddViewer(T viewer, string? code = null)
    {
        if (_bannedList.Contains(viewer.UserId)) throw new ViewerBannedException();
        if (string.IsNullOrEmpty(Code) && Code != code) throw new InvalidCodeException();
        UpdateActivity();
        if (_viewersList.Any(x => x.UserId == viewer.UserId)) throw new ViewerAlreadyExistsException();
        viewer.Online = true;
        _viewersList.Add(viewer);
    }
}