using System.Text.RegularExpressions;
using Overoom.Domain.Abstractions;
using Overoom.Domain.Rooms.BaseRoom.Exceptions;

namespace Overoom.Domain.Rooms.BaseRoom.Entities;

public abstract partial class Viewer : Entity
{
    protected Viewer(int id, string name, Uri avatarUri) : base(id)
    {
        if (MyRegex().IsMatch(name)) Name = name;
        else throw new ViewerInvalidNicknameException();
        AvatarUri = avatarUri;
    }

    public string Name { get; }
    public Uri AvatarUri { get; }
    public bool Online { get; set; }
    public bool OnPause { get; set; } = true;
    public TimeSpan TimeLine { get; set; } = TimeSpan.Zero;

    [GeneratedRegex("^[a-zA-Zа-яА-Я0-9_ ]{3,20}$")]
    private static partial Regex MyRegex();
}