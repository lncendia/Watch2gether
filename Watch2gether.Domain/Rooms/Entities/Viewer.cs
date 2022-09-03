using System.Text.RegularExpressions;
using Watch2gether.Domain.Users.Exceptions;

namespace Watch2gether.Domain.Rooms.Entities;

public class Viewer
{
    public Viewer(string name, Guid roomId, string avatarFileName)
    {
        Id = Guid.NewGuid();
        _name = name;
        RoomId = roomId;
        AvatarFileName = avatarFileName;
    }

    public Guid Id { get; }
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if (Regex.IsMatch(value, "^[a-zA-Zа-яА-Я0-9_ ]{3,20}$")) _name = value;
            else throw new InvalidNicknameException(value);
        }
    }
    public string AvatarFileName { get; set; }
    public bool Online { get; set; } = true;
    public bool OnPause { get; set; } = true;
    public TimeSpan TimeLine { get; set; } = TimeSpan.Zero;
    public Guid RoomId { get; }
}