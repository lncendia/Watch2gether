namespace Overoom.Application.Abstractions.Rooms.DTOs;

public abstract class ViewerDto
{
    protected ViewerDto(string username, int id, Uri avatarUrl, TimeSpan time, bool pause, bool fullScreen,
        AllowsDto allows)
    {
        Username = username;
        Id = id;
        AvatarUrl = avatarUrl;
        Time = time;
        Pause = pause;
        FullScreen = fullScreen;
        Allows = allows;
    }

    public int Id { get; }
    public string Username { get; }
    public Uri AvatarUrl { get; }
    public bool Pause { get; }
    public bool FullScreen { get; }
    public TimeSpan Time { get; }
    public AllowsDto Allows { get; }
}