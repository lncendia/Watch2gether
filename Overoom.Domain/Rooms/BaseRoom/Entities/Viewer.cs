using System.Text.RegularExpressions;
using Overoom.Domain.Users.Exceptions;

namespace Overoom.Domain.Rooms.BaseRoom.Entities;

public abstract class Viewer
{
    protected Viewer(string name, Guid roomId, string avatarFileName)
    {
        Id = Guid.NewGuid();
        if (Regex.IsMatch(name, "^[a-zA-Zа-яА-Я0-9_ ]{3,20}$")) Name = name;
        else throw new ViewerInvalidNicknameException(name);
        RoomId = roomId;
        AvatarFileName = avatarFileName;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string AvatarFileName { get; }
    public bool Online { get; set; }
    public bool OnPause { get; set; } = true;
    public TimeSpan TimeLine { get; set; } = TimeSpan.Zero;
    public Guid RoomId { get; }
}