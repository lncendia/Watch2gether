namespace Overoom.Application.Abstractions.StartPage.DTOs;

public class CommentStartPageDto
{
    public CommentStartPageDto(string name, string text, DateTime dateTime, Guid filmId, string filmName, string avatar)
    {
        Name = name;
        Text = text;
        DateTime = dateTime;
        FilmId = filmId;
        Avatar = avatar;
        FilmName = filmName;
    }

    public string Name { get; }
    public string Text { get; }
    public string Avatar { get; }
    public DateTime DateTime { get; }
    public Guid FilmId { get; }
    public string FilmName { get; }
}