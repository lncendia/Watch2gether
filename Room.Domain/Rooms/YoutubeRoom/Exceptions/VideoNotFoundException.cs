namespace Overoom.Domain.Rooms.YoutubeRoom.Exceptions;

public class VideoNotFoundException : Exception
{
    public VideoNotFoundException() : base("Video not found")
    {
    }
}