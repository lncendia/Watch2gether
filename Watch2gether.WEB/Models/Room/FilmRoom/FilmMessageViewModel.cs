namespace Watch2gether.WEB.Models.Room.FilmRoom;

public class FilmMessageViewModel : MessageViewModel
{
    public new FilmViewerViewModel Viewer => (FilmViewerViewModel) base.Viewer;

    public FilmMessageViewModel(string text, DateTime createdAt, ViewerViewModel viewer) : base(text, createdAt, viewer)
    {
    }
}