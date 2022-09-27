namespace Watch2gether.WEB.Models.Room.FilmRoom;

public class FilmRoomViewModel : BaseRoomViewModel
{
    public FilmRoomViewModel(List<MessageViewModel> messages, List<ViewerViewModel> viewers, FilmViewModel film,
        string connectUrl, Guid ownerId, ViewerViewModel currentViewer) : base(messages, viewers, connectUrl, ownerId,
        currentViewer)
    {
        Film = film;
    }

    public FilmViewModel Film { get; }
}

public class FilmViewModel
{
    public FilmViewModel(string name, string url)
    {
        Name = name;
        Url = url;
    }

    public string Name { get; }
    public string Url { get; }
}