namespace Overoom.WEB.Hubs.Models;

public class ViewerModel
{
    public ViewerModel(int id, string username, Uri avatar, int time)
    {
        Id = id;
        Username = username;
        Avatar = avatar;
        Time = time;
    }

    public int Id { get; }
    public string Username { get; }
    public int Time { get; }
    public Uri Avatar { get; }
}