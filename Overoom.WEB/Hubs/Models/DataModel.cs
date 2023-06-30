namespace Overoom.WEB.Hubs.Models;

public class DataModel
{
    public int Id { get; }
    public Guid RoomId { get; }
    public string RoomIdString { get; }
    public string Username { get; }
    public Uri AvatarUri { get; }

    public DataModel(int id, Guid roomId, string username, Uri avatarUri)
    {
        Id = id;
        RoomIdString = roomId.ToString();
        RoomId = roomId;
        Username = username;
        AvatarUri = avatarUri;
    }
}