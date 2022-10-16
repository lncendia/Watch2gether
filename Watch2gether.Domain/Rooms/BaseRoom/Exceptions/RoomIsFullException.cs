namespace Watch2gether.Domain.Rooms.BaseRoom.Exceptions;

public class RoomIsFullException : Exception
{
    public RoomIsFullException() : base("Room is full")
    {
    }
}