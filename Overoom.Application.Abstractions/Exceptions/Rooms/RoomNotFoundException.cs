namespace Overoom.Application.Abstractions.Exceptions.Rooms;

public class RoomNotFoundException : Exception
{
    public RoomNotFoundException() : base("Can't find room.")
    {
    }
}