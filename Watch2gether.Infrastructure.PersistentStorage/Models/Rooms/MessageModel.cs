namespace Watch2gether.Infrastructure.PersistentStorage.Models.Rooms;

public class MessageModel
{
    public int Id { get; set; }
    public Guid ViewerId { get; set; }
    public ViewerModel Viewer { get; set; } = null!;

    public Guid RoomId { get; set; }
    public RoomBaseModel Room { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public string Text { get; set; } = null!;
}