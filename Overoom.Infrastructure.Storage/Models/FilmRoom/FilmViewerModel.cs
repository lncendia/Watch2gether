using Overoom.Infrastructure.Storage.Models.Abstractions;

namespace Overoom.Infrastructure.Storage.Models.FilmRoom;

public class FilmViewerModel : IEntityModel
{
    public long Id { get; set; }
    public int EntityId { get; set; }

    public string Name { get; set; } = null!;
    public string NameNormalized { get; set; } = null!;
    public Guid RoomId { get; set; }
    public FilmRoomModel Room { get; set; } = null!;
    public Uri AvatarUri { get; set; } = null!;
    public bool Online { get; set; }
    public bool Pause { get; set; }
    public bool FullScreen { get; set; }
    public TimeSpan TimeLine { get; set; }
    public int Season { get; set; }
    public int Series { get; set; }

    public bool Beep { get; set; }
    public bool Scream { get; set; }
    public bool Change { get; set; }
}