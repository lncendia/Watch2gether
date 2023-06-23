namespace Overoom.WEB.Models.Home;

public class StartPageViewModel
{
    public StartPageViewModel(IReadOnlyCollection<CommentStartPageViewModel> comments,
        IReadOnlyCollection<FilmStartPageViewModel> films, IReadOnlyCollection<RoomStartPageViewModel> rooms)
    {
        Comments = comments;
        Films = films;
        Rooms = rooms;
    }

    public IReadOnlyCollection<CommentStartPageViewModel> Comments { get; }
    public IReadOnlyCollection<FilmStartPageViewModel> Films { get; }
    public IReadOnlyCollection<RoomStartPageViewModel> Rooms { get; }
}