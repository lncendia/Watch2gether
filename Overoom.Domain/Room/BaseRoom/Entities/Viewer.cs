using System.Text.RegularExpressions;
using Overoom.Domain.Abstractions;
using Overoom.Domain.Room.BaseRoom.Exceptions;

namespace Overoom.Domain.Room.BaseRoom.Entities;

public abstract class Viewer : Entity
{
    protected Viewer(int id, string name, string avatarFileName) : base(id)
    {
        if (Regex.IsMatch(name, "^[a-zA-Zа-яА-Я0-9_ ]{3,20}$")) Name = name;
        else throw new ViewerInvalidNicknameException(name);
        AvatarFileName = avatarFileName;
    }

    public string Name { get; }
    public string AvatarFileName { get; }
    public bool Online { get; set; }
    public bool OnPause { get; set; } = true;
    public TimeSpan TimeLine { get; set; } = TimeSpan.Zero;
}