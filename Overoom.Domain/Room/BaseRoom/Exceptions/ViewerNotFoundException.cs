namespace Overoom.Domain.Room.BaseRoom.Exceptions;

public class ViewerNotFoundException : Exception
{
    public ViewerNotFoundException() : base($"A viewer is not found in this room.")
    {
    }
}