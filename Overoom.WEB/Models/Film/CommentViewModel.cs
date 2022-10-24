namespace Overoom.WEB.Models.Film;

public class CommentViewModel
{
    public CommentViewModel(string username, string text, DateTime createdAt, string avatarFileName)
    {
        Username = username;
        Text = text;
        CreatedAt = createdAt.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
        AvatarFileName = avatarFileName;
    }

    public string Username { get; }
    public string Text { get; }
    public string AvatarFileName { get; }
    public string CreatedAt { get; }
}