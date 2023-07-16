namespace Overoom.Application.Abstractions.StartPage.DTOs;

public class CommentDto
{
    public CommentDto(string name, string text, DateTime dateTime, Guid filmId, Uri avatarUri)
    {
        Name = name;
        Text = text;
        DateTime = dateTime;
        FilmId = filmId;
        AvatarUri = avatarUri;
    }

    public string Name { get; }
    public string Text { get; }
    public Uri AvatarUri { get; }
    public DateTime DateTime { get; }
    public Guid FilmId { get; }
}