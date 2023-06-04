namespace Overoom.Domain.Room.YoutubeRoom.Exceptions;

public class VideoNotFoundException : Exception
{
    public VideoNotFoundException() : base("Video not found")
    {
    }
}