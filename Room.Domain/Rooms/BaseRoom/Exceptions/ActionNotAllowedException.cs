namespace Room.Domain.Rooms.BaseRoom.Exceptions;

public class ActionNotAllowedException : Exception
{
    public ActionNotAllowedException() : base("The user has forbidden this action")
    {
    }
}