namespace Overoom.Infrastructure.Storage.Models.Rooms.YoutubeRoom;

public class VideoIdModel
{
    public long Id { get; set; }
    public string VideoId { get; set; } = null!;
    public Guid RoomId { get; set; }
    public YoutubeRoomModel Room { get; set; } = null!;
}