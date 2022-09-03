namespace Watch2gether.Infrastructure.PersistentStorage.Models;

public class MessageModel
{
    public Guid Id { get; set; }
    public Guid ViewerId { get; set; }
    public ViewerModel Viewer { get; set; } = null!;

    public Guid RoomId { get; set; }
    public RoomModel Room { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public string Text { get; set; } = null!;
}