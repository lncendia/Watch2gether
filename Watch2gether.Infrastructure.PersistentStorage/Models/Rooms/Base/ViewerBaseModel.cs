namespace Watch2gether.Infrastructure.PersistentStorage.Models.Rooms.Base;

public class ViewerBaseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid RoomId { get; set; }
    public RoomBaseModel Room { get; set; } = null!;
    public string AvatarFileName { get; set; } = null!;
    public bool Online { get; set; }
    public bool OnPause { get; set; }
    public TimeSpan TimeLine { get; set; }
}