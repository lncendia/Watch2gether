using Overoom.Infrastructure.Storage.Models.Abstractions;

namespace Overoom.Infrastructure.Storage.Models.YoutubeRoom;

public class YoutubeViewerModel : IEntityModel
{
    public long Id { get; set; }
    public int EntityId { get; set; }

    public string Name { get; set; } = null!;
    public string NameNormalized { get; set; } = null!;
    public Guid RoomId { get; set; }
    public YoutubeRoomModel Room { get; set; } = null!;
    public Uri AvatarUri { get; set; } = null!;
    public bool Online { get; set; }
    public bool OnPause { get; set; }
    public TimeSpan TimeLine { get; set; }
    public string CurrentVideoId { get; set; } = null!;
}