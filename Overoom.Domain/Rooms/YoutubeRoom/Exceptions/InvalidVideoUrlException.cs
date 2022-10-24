namespace Overoom.Domain.Rooms.YoutubeRoom.Exceptions;

public class InvalidVideoUrlException : Exception
{
    public InvalidVideoUrlException() : base("Invalid video url")
    {
    }
}