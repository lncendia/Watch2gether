using Overoom.Domain.Rooms.YoutubeRoom.Exceptions;
using Room.Domain.Rooms.BaseRoom.Entities;
using Room.Domain.Rooms.BaseRoom.Exceptions;
using Room.Domain.Rooms.YoutubeRoom.ValueObjects;
using Room.Domain.Users.Entities;

namespace Room.Domain.Rooms.YoutubeRoom.Entities;

public class YoutubeRoom : Room<YoutubeViewer>
{
    public YoutubeRoom(User user, Uri url, bool isOpen, bool videoAccess) : base(new YoutubeViewer(user, 1),
        isOpen)
    {
        VideoAccess = videoAccess;
        _videos.Add(new Video(1, url));
    }


    public bool VideoAccess { get; }

    private readonly List<Video> _videos = [];
    public IReadOnlyCollection<Video> Videos => _videos.OrderBy(v => v.OrderNumber).ToArray();

    public override void Connect(User viewer, string? code = null)
    {
        var youtubeViewer = new YoutubeViewer(viewer, Owner.CurrentVideoNumber);
        AddViewer(youtubeViewer, code);
    }

    public void ChangeVideo(Guid target, int videoNumber)
    {
        var viewer = GetViewer(target);
        if (Videos.Last().OrderNumber < videoNumber) throw new VideoNotFoundException();
        viewer.CurrentVideoNumber = videoNumber;
    }

    public void AddVideo(Guid initiator, Uri url)
    {
        if (initiator != Owner.UserId && !VideoAccess) throw new ActionNotAllowedException();
        _videos.Add(new Video(Videos.Last().OrderNumber + 1, url));
    }

    public void RemoveVideo(Guid initiator, int videoNumber)
    {
        if (initiator != Owner.UserId && !VideoAccess) throw new ActionNotAllowedException();
        if (Videos.Last().OrderNumber < videoNumber) throw new VideoNotFoundException();
        _v
    }

    public string ChangeVideo(Guid initiator, Uri url)
    {
        if (initiator != Owner.UserId && !VideoAccess) throw new ActionNotAllowedException();
        var id = GetId(url);
        _videos.Add(id);
        return id;
    }
}