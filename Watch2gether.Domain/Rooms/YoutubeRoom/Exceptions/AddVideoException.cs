namespace Watch2gether.Domain.Rooms.YoutubeRoom.Exceptions;

public class AddVideoException : Exception
{
    public AddVideoException() : base("The viewer is not the owner of the room")
    {
    }
}