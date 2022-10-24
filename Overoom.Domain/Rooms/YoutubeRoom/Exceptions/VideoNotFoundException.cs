namespace Overoom.Domain.Rooms.BaseRoom.Exceptions;

public class VideoNotFoundException : Exception
{
    public VideoNotFoundException() : base("Video not found")
    {
    }
}