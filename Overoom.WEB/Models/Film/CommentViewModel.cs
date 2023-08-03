namespace Overoom.WEB.Models.Film;

public class CommentViewModel
{
    public CommentViewModel(string username, string text, DateTime createdAt, Uri avatarUri, Guid id, bool isUserComment)
    {
        Username = username;
        Text = text;
        CreatedAt = createdAt.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
        AvatarUri = avatarUri;
        Id = id;
        IsUserComment = isUserComment;
    }

    public Guid Id { get; }
    public string Username { get; }
    public string Text { get; }
    public Uri AvatarUri { get; }
    public string CreatedAt { get; }
    public bool IsUserComment { get; }
}