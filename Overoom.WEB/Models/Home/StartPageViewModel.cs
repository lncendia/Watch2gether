namespace Overoom.WEB.Models.Home;

public class StartPageViewModel
{
    public StartPageViewModel(IEnumerable<CommentStartPageViewModel> comments, IEnumerable<FilmStartPageViewModel> films, IEnumerable<RoomStartPageViewModel> rooms)
    {
        Comments = comments.ToList();
        Films = films.ToList();
        Rooms = rooms.ToList();
    }

    public List<CommentStartPageViewModel> Comments { get; }
    public List<FilmStartPageViewModel> Films { get; }
    public List<RoomStartPageViewModel> Rooms { get; }
}