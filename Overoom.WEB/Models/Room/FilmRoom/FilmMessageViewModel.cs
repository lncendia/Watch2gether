namespace Overoom.WEB.Models.Room.FilmRoom;

public class FilmMessageViewModel : MessageViewModel
{
    public new FilmViewerViewModel Viewer => (FilmViewerViewModel) base.Viewer;

    public FilmMessageViewModel(string text, DateTime createdAt, FilmViewerViewModel viewer) : base(text, createdAt, viewer)
    {
    }
}