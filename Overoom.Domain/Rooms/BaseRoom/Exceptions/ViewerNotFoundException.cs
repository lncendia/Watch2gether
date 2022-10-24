namespace Overoom.Domain.Rooms.BaseRoom.Exceptions;

public class ViewerNotFoundException : Exception
{
    public ViewerNotFoundException() : base($"A viewer is not found in this room.")
    {
    }
}