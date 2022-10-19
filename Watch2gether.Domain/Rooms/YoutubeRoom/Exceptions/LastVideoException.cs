namespace Watch2gether.Domain.Rooms.YoutubeRoom.Exceptions;

public class LastVideoException:Exception
{
    public LastVideoException():base("Cannot remove the last video from a room")
    {
    }
}