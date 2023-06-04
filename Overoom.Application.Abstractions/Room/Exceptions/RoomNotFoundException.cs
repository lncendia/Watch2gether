namespace Overoom.Application.Abstractions.Room.Exceptions;

public class RoomNotFoundException : Exception
{
    public RoomNotFoundException() : base("Can't find room.")
    {
    }
}