using System.Text.RegularExpressions;
using Room.Domain.Rooms.BaseRoom.Exceptions;
using Room.Domain.Users.Entities;

namespace Room.Domain.Rooms.BaseRoom.Entities;

public abstract partial class Viewer(User user)
{
    public Guid UserId { get; } = user.Id;
    public bool Online { get; internal set; } = true;
    public bool FullScreen { get; internal set; }
    public bool Pause { get; internal set; } = true;
    public TimeSpan TimeLine { get; internal set; } = TimeSpan.Zero;

    private string? _name;

    public string? CustomName
    {
        get => _name;
        internal set
        {
            if (value == null) _name = null;
            else if (MyRegex().IsMatch(value)) _name = value;
            else throw new ViewerInvalidNicknameException();
        }
    }

    [GeneratedRegex("^[a-zA-Zа-яА-Я0-9_ ]{1,20}$")]
    private static partial Regex MyRegex();
}