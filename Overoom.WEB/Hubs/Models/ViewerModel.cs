namespace Overoom.WEB.Hubs.Models;

public class ViewerModel
{
    public ViewerModel(int id, string username, Uri avatar, int time, bool beep, bool scream, bool change)
    {
        Id = id;
        Username = username;
        Avatar = avatar;
        Time = time;
        Beep = beep;
        Scream = scream;
        Change = change;
    }

    public int Id { get; }
    public string Username { get; }
    public int Time { get; }
    public Uri Avatar { get; }

    public bool Beep { get; }
    public bool Scream { get; }
    public bool Change { get; }
}