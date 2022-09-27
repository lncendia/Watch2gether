namespace Watch2gether.Domain.Rooms.YoutubeRoom;

public class YoutubeRoom : BaseRoom.BaseRoom
{
    public YoutubeRoom(string link, string name, string avatarFileName) : base(name, avatarFileName) =>
        Link = new Uri(link);

    public Uri Link { get; }
}