namespace Overoom.WEB.Hubs.Models;

public class ViewerModel
{
    public ViewerModel(Guid id, string username, string avatar, int time)
    {
        Id = id;
        Username = username;
        Avatar = avatar;
        Time = time;
    }

    public Guid Id { get; }
    public string Username { get; }
    public int Time { get; }
    public string Avatar { get; }
}