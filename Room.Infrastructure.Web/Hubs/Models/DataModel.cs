namespace Room.Infrastructure.Web.Hubs.Models;

public class DataModel(int id, Guid roomId)
{
    public int Id { get; } = id;
    public Guid RoomId { get; } = roomId;
    public string RoomIdString { get; } = roomId.ToString();
}