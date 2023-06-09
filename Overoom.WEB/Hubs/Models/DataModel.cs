namespace Overoom.WEB.Hubs.Models;

public class DataModel
{
    public Guid Id { get; }
    public Guid RoomId { get; }
    public string RoomIdString { get; }
    public string Username { get; }
    public string AvatarUri { get; }

    public DataModel(Guid id, string roomId, string username, string avatarUri)
    {
        Id = id;
        RoomIdString = roomId;
        RoomId = Guid.Parse(roomId);
        Username = username;
        AvatarUri = avatarUri;
    }
}