namespace Films.Infrastructure.Web.Models.Rooms;

public class MessageViewModel
{
    public MessageViewModel(string text, DateTime createdAt, int viewerId, Uri avatarUri, string username)
    {
        Text = text;
        ViewerId = viewerId;
        AvatarUri = avatarUri;
        Username = username;
        CreatedAt = createdAt.ToLocalTime().ToString("T");
    }

    public string Username { get; }
    public int ViewerId { get; }
    public Uri AvatarUri { get; }
    public string CreatedAt { get; }
    public string Text { get; }
}
