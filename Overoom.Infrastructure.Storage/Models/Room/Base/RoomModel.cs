using Overoom.Infrastructure.Storage.Models.Abstractions;

namespace Overoom.Infrastructure.Storage.Models.Room.Base;

public class RoomModel : IAggregateModel
{
    public Guid Id { get; set; }
    public bool IsOpen { get; set; }
    public int IdCounter { get; set; }
    public List<MessageModel> Messages { get; set; } = new();
    public List<ViewerModel> Viewers { get; set; } = new();
    public int OwnerId { get; set; }
    public DateTime LastActivity { get; set; }
}