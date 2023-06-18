namespace Overoom.Application.Abstractions.Rooms.Exceptions;

public class RoomNotFoundException : Exception
{
    public RoomNotFoundException() : base("Can't find room.")
    {
    }
}