using System.Text.RegularExpressions;
using Watch2gether.Domain.Users.Exceptions;

namespace Watch2gether.Domain.Rooms.BaseRoom.Entities;

public abstract class Viewer
{
    public Viewer(string name, Guid roomId, string avatarFileName)
    {
        Id = Guid.NewGuid();
        if (Regex.IsMatch(name, "^[a-zA-Zа-яА-Я0-9_ ]{3,20}$")) Name = name;
        else throw new InvalidNicknameException(name);
        RoomId = roomId;
        AvatarFileName = avatarFileName;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string AvatarFileName { get; }
    public bool Online { get; set; } = true;
    public bool OnPause { get; set; } = true;
    public TimeSpan TimeLine { get; set; } = TimeSpan.Zero;
    public Guid RoomId { get; }
}