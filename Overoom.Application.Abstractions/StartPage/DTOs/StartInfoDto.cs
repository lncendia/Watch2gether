namespace Overoom.Application.Abstractions.StartPage.DTOs;

public class StartInfoDto
{
    public StartInfoDto(IEnumerable<CommentStartPageDto> comments, IEnumerable<FilmStartPageDto> films, IEnumerable<RoomStartPageDto> rooms)
    {
        Comments = comments.ToList();
        Films = films.ToList();
        Rooms = rooms.ToList();
    }

    public List<CommentStartPageDto> Comments { get; }
    public List<FilmStartPageDto> Films { get; }
    public List<RoomStartPageDto> Rooms { get; }
}