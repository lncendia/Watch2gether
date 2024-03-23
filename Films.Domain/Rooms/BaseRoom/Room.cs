using System.Text;
using Films.Domain.Abstractions;
using Films.Domain.Rooms.BaseRoom.Exceptions;
using Films.Domain.Servers;
using Films.Domain.Users;

namespace Films.Domain.Rooms.BaseRoom;

/// <summary> 
/// Класс, представляющий комнату. 
/// </summary> 
public abstract class Room : AggregateRoot
{
    protected Room(User user, IEnumerable<Server> servers, bool isOpen)
    {
        var server = servers
            .Where(s => s.IsEnabled)
            .Where(s => s.RoomsCount < s.MaxRoomsCount)
            .MinBy(s => s.RoomsCount);

        if (server == null) throw new NoSuitableServerException();
        ServerId = server.Id;

        if (!isOpen) Code = GenerateRandomCode(5);
        _viewers.Add(user.Id);
        CreationDate = DateTime.UtcNow;
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

    /// <summary>
    /// Секретный код комнаты
    /// </summary>
    public string? Code { get; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreationDate { get; }

    /// <summary> 
    /// Участники комнаты
    /// </summary> 
    private readonly List<Guid> _viewers = [];

    /// <summary> 
    /// Участники комнаты
    /// </summary> 
    public IReadOnlyCollection<Guid> Viewers => _viewers;


    /// <summary> 
    /// Заблокированные пользователи
    /// </summary> 
    public IReadOnlyCollection<Guid> Banned => _bannedUsers;

    /// <summary> 
    /// Заблокированные пользователи
    /// </summary> 
    private readonly List<Guid> _bannedUsers = [];

    /// <summary> 
    /// Сервер.
    /// </summary> 
    public Guid ServerId { get; }

    public virtual void Connect(User user, string? code)
    {
        if (_viewers.Any(v => v == user.Id)) return;

        if (Code != null)
        {
            if (!string.Equals(Code, code, StringComparison.CurrentCultureIgnoreCase)) throw new InvalidCodeException();
        }

        if (_bannedUsers.Any(v => v == user.Id)) throw new UserBannedInRoomException();
        if (_viewers.Count >= 10) throw new RoomIsFullException();
        _viewers.Add(user.Id);
    }

    public virtual void Disconnect(Guid userId)
    {
        var viewer = GetViewer(userId);
        _bannedUsers.Add(viewer);
        _viewers.Remove(viewer);
    }

    public void Block(Guid userId)
    {
        var viewer = GetViewer(userId);
        _bannedUsers.Add(viewer);
        _viewers.Remove(viewer);
    }

    private Guid GetViewer(Guid userId)
    {
        var viewer = _viewers.FirstOrDefault(x => x == userId);
        if (viewer == default) throw new ViewerNotFoundException();
        return viewer;
    }
}