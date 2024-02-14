namespace Overoom.Infrastructure.Web.Hubs.Models;

public class ViewerModel(int id, string username, Uri avatar, int time, bool beep, bool scream, bool change)
{
    public int Id { get; } = id;
    public string Username { get; } = username;
    public int Time { get; } = time;
    public Uri Avatar { get; } = avatar;

    public bool Beep { get; } = beep;
    public bool Scream { get; } = scream;
    public bool Change { get; } = change;
}