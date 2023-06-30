namespace Overoom.WEB.Models.Home;

public class CommentStartPageViewModel
{
    public CommentStartPageViewModel(string name, string text, DateTime dateTime, Guid filmId, Uri avatarUri)
    {
        Name = name;
        Text = text;
        DateTime = dateTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
        FilmId = filmId;
        AvatarUri = avatarUri;
    }

    public string Name { get; }
    public string Text { get; }
    public Uri AvatarUri { get; }
    public string DateTime { get; }
    public Guid FilmId { get; }
}