namespace Watch2gether.Application.Abstractions.Exceptions.Rooms;

public class RoomNotFoundException : Exception
{
    public RoomNotFoundException() : base("Can't find room.")
    {
    }
}