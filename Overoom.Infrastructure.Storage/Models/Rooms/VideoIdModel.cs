namespace Overoom.Infrastructure.Storage.Models.Rooms;

public class VideoIdModel
{
    public int Id { get; set; }
    public string VideoId { get; set; } = null!;
    public Guid RoomId { get; set; }
    public YoutubeRoomModel Room { get; set; } = null!;
}