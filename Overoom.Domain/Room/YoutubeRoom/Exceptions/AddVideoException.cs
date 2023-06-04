namespace Overoom.Domain.Room.YoutubeRoom.Exceptions;

public class AddVideoException : Exception
{
    public AddVideoException() : base("The viewer is not the owner of the room")
    {
    }
}