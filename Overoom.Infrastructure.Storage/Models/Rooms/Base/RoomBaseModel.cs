using Overoom.Infrastructure.Storage.Models.Abstractions;

namespace Overoom.Infrastructure.Storage.Models.Rooms.Base;

public class RoomBaseModel : IAggregateModel
{
    public Guid Id { get; set; }
    public bool IsOpen { get; set; }
    public List<MessageModel> Messages { get; set; } = new();
    public Guid OwnerId { get; set; }
    public DateTime LastActivity { get; set; }
}