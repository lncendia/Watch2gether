namespace Watch2gether.WEB.Hubs.Models;

public class DataModel
{
    public Guid Id { get; }
    public Guid RoomId { get; }
    public string RoomIdString { get; }
    public string Username { get; }
    public string AvatarFileName { get; }

    public DataModel(Guid id, string roomId, string username, string avatarFileName)
    {
        Id = id;
        RoomIdString = roomId;
        RoomId = Guid.Parse(roomId);
        Username = username;
        AvatarFileName = avatarFileName;
    }
}