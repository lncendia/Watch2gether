using Overoom.Domain.Users.Entities;

namespace Overoom.Domain.Rooms.BaseRoom.DTOs;

public class ViewerDto
{
    public ViewerDto(string name, Uri avatarUri)
    {
        Name = name;
        AvatarUri = avatarUri;
        Beep = true;
        Scream = true;
        Change = true;
    }

    public ViewerDto(User user)
    {
        Name = user.Name;
        AvatarUri = user.AvatarUri;
        Beep = user.Allows.Beep;
        Scream = user.Allows.Scream;
        Change = user.Allows.Change;
    }

    public string Name { get; }
    public Uri AvatarUri { get; }
    public bool Beep { get; }
    public bool Scream { get; }
    public bool Change { get; }
}