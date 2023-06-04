namespace Overoom.Domain.Room.YoutubeRoom.Exceptions;

public class InvalidVideoUrlException : Exception
{
    public InvalidVideoUrlException() : base("Invalid video url")
    {
    }
}