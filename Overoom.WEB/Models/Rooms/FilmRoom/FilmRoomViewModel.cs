using Overoom.Domain.Films.Enums;

namespace Overoom.WEB.Models.Rooms.FilmRoom;

public class FilmRoomViewModel : BaseRoomViewModel
{
    public FilmRoomViewModel(IReadOnlyCollection<MessageViewModel> messages,
        IReadOnlyCollection<FilmViewerViewModel> viewers, FilmViewModel film,
        string connectUrl, int ownerId, int currentViewerId) : base(messages, viewers, connectUrl, ownerId,
        currentViewerId)
    {
        Film = film;
    }

    public FilmViewModel Film { get; }
    public new IReadOnlyCollection<FilmViewerViewModel> Viewers => base.Viewers.Cast<FilmViewerViewModel>().ToList();
}

public class FilmViewModel
{
    public FilmViewModel(string name, Uri url, FilmType type, CdnType cdnType)
    {
        Name = name;
        Url = url;
        Type = type;
        CdnType = cdnType;
    }

    public string Name { get; }
    public Uri Url { get; }
    public CdnType CdnType { get; }
    public FilmType Type { get; }
}