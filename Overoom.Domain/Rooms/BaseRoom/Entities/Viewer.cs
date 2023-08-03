using System.Text.RegularExpressions;
using Overoom.Domain.Abstractions;
using Overoom.Domain.Rooms.BaseRoom.DTOs;
using Overoom.Domain.Rooms.BaseRoom.Exceptions;
using Overoom.Domain.Rooms.BaseRoom.ValueObjects;

namespace Overoom.Domain.Rooms.BaseRoom.Entities;

public abstract partial class Viewer : Entity
{
    protected Viewer(int id, ViewerDto viewer) : base(id)
    {
        Name = viewer.Name;
        AvatarUri = viewer.AvatarUri;
        Allows = new Allows(viewer.Beep, viewer.Scream, viewer.Change);
    }

    private string _name = null!;

    public string Name
    {
        get => _name;
        internal set
        {
            if (MyRegex().IsMatch(value)) _name = value;
            else throw new ViewerInvalidNicknameException();
        }
    }

    public Uri AvatarUri { get; }
    public bool Online { get; set; }
    public bool FullScreen { get; set; }
    public bool Pause { get; set; } = true;
    public TimeSpan TimeLine { get; set; } = TimeSpan.Zero;
    public Allows Allows { get; }

    [GeneratedRegex("^[a-zA-Zа-яА-Я0-9_ ]{3,20}$")]
    private static partial Regex MyRegex();
}