namespace Watch2gether.Infrastructure.PersistentStorage.Models;

public class RoomModel
{
    public Guid Id { get; set; }
    public Guid FilmId { get; set; }
    public FilmModel Film { get; set; } = null!;
    public List<ViewerModel> Viewers { get; set; } = new();
    public List<MessageModel> Messages { get; set; } = new();
    public Guid OwnerId { get; set; }
    public DateTime LastActivity { get; set; }
}