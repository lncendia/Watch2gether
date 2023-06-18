using System.Text.RegularExpressions;
using Overoom.Domain.Abstractions;
using Overoom.Domain.Rooms.BaseRoom.Exceptions;

namespace Overoom.Domain.Rooms.BaseRoom.Entities;

public abstract class Viewer : Entity
{
    protected Viewer(int id, string name, string avatarUri) : base(id)
    {
        if (Regex.IsMatch(name, "^[a-zA-Zа-яА-Я0-9_ ]{3,20}$")) Name = name;
        else throw new ViewerInvalidNicknameException(name);
        AvatarUri = avatarUri;
    }

    public string Name { get; }
    public string AvatarUri { get; }
    public bool Online { get; set; }
    public bool OnPause { get; set; } = true;
    public TimeSpan TimeLine { get; set; } = TimeSpan.Zero;
}