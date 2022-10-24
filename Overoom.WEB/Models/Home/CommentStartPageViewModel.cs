namespace Overoom.WEB.Models.Home;

public class CommentStartPageViewModel
{
    public CommentStartPageViewModel(string name, string text, DateTime dateTime, Guid filmId, string avatar)
    {
        Name = name;
        Text = text;
        DateTime = dateTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
        FilmId = filmId;
        Avatar = avatar;
    }

    public string Name { get; }
    public string Text { get; }
    public string Avatar { get; }
    public string DateTime { get; }
    public Guid FilmId { get; }
}