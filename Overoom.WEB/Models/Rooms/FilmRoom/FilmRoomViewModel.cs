namespace Overoom.WEB.Models.Rooms.FilmRoom;

public class FilmRoomViewModel : BaseRoomViewModel
{
    public FilmRoomViewModel(IReadOnlyCollection<MessageViewModel> messages,
        IReadOnlyCollection<FilmViewerViewModel> viewers, FilmViewModel film, string connectUrl, int ownerId,
        int currentViewerId, bool isOpen) : base(messages, viewers, connectUrl, ownerId, currentViewerId, isOpen)
    {
        Film = film;
    }

    public FilmViewModel Film { get; }
    public new IReadOnlyCollection<FilmViewerViewModel> Viewers => base.Viewers.Cast<FilmViewerViewModel>().ToList();
}