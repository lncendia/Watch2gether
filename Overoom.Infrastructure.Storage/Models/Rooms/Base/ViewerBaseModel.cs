using Overoom.Infrastructure.Storage.Models.Abstractions;

namespace Overoom.Infrastructure.Storage.Models.Rooms.Base;

public class ViewerBaseModel : IEntityModel
{
    public long Id { get; set; }
    public int EntityId { get; set; }

    public string Name { get; set; } = null!;
    public Guid RoomId { get; set; }
    public RoomBaseModel Room { get; set; } = null!;
    public string AvatarFileName { get; set; } = null!;
    public bool Online { get; set; }
    public bool OnPause { get; set; }
    public TimeSpan TimeLine { get; set; }
}