using Overoom.Infrastructure.Storage.Models.Room.YoutubeRoom;

namespace Overoom.Infrastructure.Storage.Models.YoutubeRoom;

public class YoutubeMessageModel
{
    public long Id { get; set; }
    public int ViewerEntityId { get; set; }
    public long ViewerId { get; set; }
    public YoutubeViewerModel Viewer { get; set; } = null!;

    public Guid RoomId { get; set; }
    public YoutubeRoomModel Room { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public string Text { get; set; } = null!;
}