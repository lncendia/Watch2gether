namespace Watch2gether.Infrastructure.PersistentStorage.Models.Rooms;

public class RoomBaseModel
{
    public Guid Id { get; set; }
    public bool IsOpen { get; set; }
    public List<ViewerModel> Viewers { get; set; } = new();
    public List<MessageModel> Messages { get; set; } = new();
    public Guid OwnerId { get; set; }
    public DateTime LastActivity { get; set; }
}