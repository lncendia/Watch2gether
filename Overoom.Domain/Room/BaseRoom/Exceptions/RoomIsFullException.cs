namespace Overoom.Domain.Room.BaseRoom.Exceptions;

public class RoomIsFullException : Exception
{
    public RoomIsFullException() : base("Room is full")
    {
    }
}