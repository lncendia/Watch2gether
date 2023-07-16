namespace Overoom.Domain.Rooms.BaseRoom.Exceptions;

public class ViewerInvalidNicknameException : Exception
{
    public ViewerInvalidNicknameException() : base(
        "Nickname must be between 3 and 20 characters long and can contain only latin or cyrillic letters, digits and underscores.")
    {
    }
}