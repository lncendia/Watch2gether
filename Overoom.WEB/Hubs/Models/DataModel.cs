namespace Overoom.WEB.Hubs.Models;

public class DataModel
{
    public int Id { get; }
    public Guid RoomId { get; }
    public string RoomIdString { get; }

    public DataModel(int id, Guid roomId)
    {
        Id = id;
        RoomIdString = roomId.ToString();
        RoomId = roomId;
    }
}