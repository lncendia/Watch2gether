using Overoom.Domain.Room.BaseRoom.Exceptions;
using Overoom.Domain.Room.YoutubeRoom.Exceptions;

namespace Overoom.Domain.Room.YoutubeRoom.Entities;

public class YoutubeRoom : BaseRoom.Entities.Room
{
    public YoutubeRoom(string url, string name, string avatarUri, bool addAccess)
    {
        AddAccess = addAccess;
        var id = GetId(url);
        _ids.Add(id);
        base.Owner = new YoutubeViewer(IdCounter, name, avatarUri, id);
        IdCounter++;
        ViewersList.Add(Owner);
    }

    public bool AddAccess { get; }
    public new YoutubeViewer Owner => (YoutubeViewer)base.Owner!;
    public IReadOnlyCollection<YoutubeViewer> Viewers => ViewersList.Cast<YoutubeViewer>().ToList().AsReadOnly();
    public IReadOnlyCollection<string> VideoIds => _ids.ToList().AsReadOnly();
    private readonly List<string> _ids = new();

    public void Connect(string name, string avatarUri)
    {
        if (ViewersList.Count >= 10) throw new RoomIsFullException();
        var viewer = new YoutubeViewer(IdCounter, name, avatarUri, Owner.CurrentVideoId);
        AddViewer(viewer);
    }

    public void ChangeVideo(int viewerId, string id)
    {
        if (!VideoIds.Contains(id)) throw new VideoNotFoundException();
        var viewer = (YoutubeViewer)GetViewer(viewerId);
        viewer.CurrentVideoId = id;
    }

    public string AddVideo(int viewerId, string url)
    {
        if (viewerId != Owner.Id && !AddAccess) throw new AddVideoException();
        var id = GetId(url);
        _ids.Add(id);
        return id;
    }

    public void RemoveId(string id)
    {
        if (_ids.Count == 1) throw new LastVideoException();
        if (ViewersList.Any(x => ((YoutubeViewer)x).CurrentVideoId == id))
            throw new VideoInViewException();
        _ids.Remove(id);
    }

    private static string GetId(string url)
    {
        string id;
        try
        {
            var uri = new Uri(url);
            id = uri.Host switch
            {
                "www.youtube.com" => uri.Query[3..],
                "youtu.be" => uri.Segments[1],
                _ => string.Empty
            };
        }
        catch
        {
            throw new InvalidVideoUrlException();
        }

        if (string.IsNullOrEmpty(id)) throw new InvalidVideoUrlException();
        return id;
    }
}