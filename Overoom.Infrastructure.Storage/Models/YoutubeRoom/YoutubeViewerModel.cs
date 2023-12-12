using Overoom.Infrastructure.Storage.Models.Abstractions;

namespace Overoom.Infrastructure.Storage.Models.YoutubeRoom;

public class YoutubeViewerModel : IEntityModel
{
    public int EntityId { get; set; }
    public Guid RoomId { get; set; }

    public string Name { get; set; } = null!;
    public YoutubeRoomModel Room { get; set; } = null!;
    public Uri AvatarUri { get; set; } = null!;
    public bool Online { get; set; }
    public bool Pause { get; set; }
    public bool FullScreen { get; set; }
    public TimeSpan TimeLine { get; set; }
    public string CurrentVideoId { get; set; } = null!;
    
    public bool Beep { get; set; }
    public bool Scream { get; set; }
    public bool Change { get; set; }
}