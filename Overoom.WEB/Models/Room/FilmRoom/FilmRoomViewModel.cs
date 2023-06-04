using Overoom.Domain.Film.Enums;

namespace Overoom.WEB.Models.Room.FilmRoom;

public class FilmRoomViewModel : BaseRoomViewModel
{
    public FilmRoomViewModel(IEnumerable<FilmMessageViewModel> messages, IEnumerable<FilmViewerViewModel> viewers, FilmViewModel film,
        string connectUrl, Guid ownerId, Guid currentViewerId) : base(messages, viewers, connectUrl, ownerId,
        currentViewerId)
    {
        Film = film;
    }

    public FilmViewModel Film { get; }
    public new List<FilmViewerViewModel> Viewers => base.Viewers.Cast<FilmViewerViewModel>().ToList();
    public new List<FilmMessageViewModel> Messages => base.Messages.Cast<FilmMessageViewModel>().ToList();

    //public new FilmViewerViewModel CurrentViewer => (FilmViewerViewModel) base.CurrentViewer;
}

public class FilmViewModel
{
    public FilmViewModel(string name, string url, FilmType type)
    {
        Name = name;
        Url = url;
        Type = type;
    }

    public string Name { get; }
    public string Url { get; }
    public FilmType Type { get; }
}