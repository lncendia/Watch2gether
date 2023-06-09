namespace Overoom.Infrastructure.Storage.Models.Rooms.Base;

public class MessageModel
{
    public long Id { get; set; }
    public int ViewerEntityId { get; set; }
    public long ViewerId { get; set; }
    public ViewerModel Viewer { get; set; } = null!;

    public Guid RoomId { get; set; }
    public RoomModel Room { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public string Text { get; set; } = null!;
}