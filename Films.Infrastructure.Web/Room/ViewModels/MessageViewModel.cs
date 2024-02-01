namespace Films.Infrastructure.Web.Models.Rooms;

public class MessageViewModel
{
    public MessageViewModel(string text, DateTime createdAt, int viewerId, Uri AvatarUrl, string username)
    {
        Text = text;
        ViewerId = viewerId;
        AvatarUrl = AvatarUrl;
        Username = username;
        CreatedAt = createdAt.ToLocalTime().ToString("T");
    }

    public string Username { get; }
    public int ViewerId { get; }
    public Uri AvatarUrl { get; }
    public string CreatedAt { get; }
    public string Text { get; }
}
