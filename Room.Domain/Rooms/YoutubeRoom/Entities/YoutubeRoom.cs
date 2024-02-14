using Room.Domain.Rooms.BaseRoom.Entities;
using Room.Domain.Rooms.BaseRoom.Exceptions;
using Room.Domain.Rooms.YoutubeRoom.Exceptions;
using Room.Domain.Rooms.YoutubeRoom.ValueObjects;
using Room.Domain.Users.Entities;

namespace Room.Domain.Rooms.YoutubeRoom.Entities;

public class YoutubeRoom : Room<YoutubeViewer>
{
    public YoutubeRoom(User user, Video video, bool isOpen, bool videoAccess) : base(new YoutubeViewer(user, video.Id),
        isOpen)
    {
        VideoAccess = videoAccess;
        _videos.Add(video);
    }


    public bool VideoAccess { get; }

    private readonly List<Video> _videos = [];
    public IReadOnlyCollection<Video> Videos => _videos.OrderBy(v => v.Added).ToArray();

    public override void Connect(User viewer, string? code = null)
    {
        var youtubeViewer = new YoutubeViewer(viewer, Owner.VideoId);
        AddViewer(youtubeViewer, code);
    }

    public void ChangeVideo(Guid target, string videoId)
    {
        var viewer = GetViewer(target);
        if (Videos.All(v => v.Id != videoId)) throw new VideoNotFoundException();
        viewer.VideoId = videoId;
    }

    public void AddVideo(Guid initiator, Video video)
    {
        if (initiator != Owner.UserId && !VideoAccess) throw new ActionNotAllowedException();
        _videos.Add(video);
    }

    public void RemoveVideo(Guid initiator, string videoId)
    {
        if (initiator != Owner.UserId && !VideoAccess) throw new ActionNotAllowedException();
        var video = _videos.FirstOrDefault(v => v.Id == videoId);
        if (video == null) throw new VideoNotFoundException();
        if (Viewers.Any(v => v.VideoId == videoId)) throw new VideoInViewException();
        _videos.Remove(video);
    }
}