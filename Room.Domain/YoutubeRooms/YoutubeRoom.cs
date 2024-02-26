using Room.Domain.BaseRoom;
using Room.Domain.BaseRoom.Exceptions;
using Room.Domain.YoutubeRooms.Entities;
using Room.Domain.YoutubeRooms.Events;
using Room.Domain.YoutubeRooms.Exceptions;
using Room.Domain.YoutubeRooms.ValueObjects;

namespace Room.Domain.YoutubeRooms;

public class YoutubeRoom : Room<YoutubeViewer>
{
    public required bool VideoAccess { get; init; }

    private readonly List<Video> _videos = [];
    public IReadOnlyCollection<Video> Videos => _videos.OrderBy(v => v.Added).ToArray();

    public override void Connect(YoutubeViewer viewer)
    {
        viewer.VideoId = Owner.VideoId;
        base.Connect(viewer);
    }

    public override void Kick(Guid initiatorId, Guid targetId)
    {
        base.Kick(initiatorId, targetId);
        AddDomainEvent(new YoutubeRoomViewerKickedDomainEvent(this, targetId));
    }
    
    public void ChangeVideo(Guid target, string videoId)
    {
        var viewer = GetViewer(target);
        if (Videos.All(v => v.Id != videoId)) throw new VideoNotFoundException();
        viewer.VideoId = videoId;
    }

    public void AddVideo(Guid initiator, Video video)
    {
        if (initiator != Owner.Id && !VideoAccess) throw new ActionNotAllowedException();
        _videos.Add(video);
    }

    public void RemoveVideo(Guid initiator, string videoId)
    {
        if (initiator != Owner.Id && !VideoAccess) throw new ActionNotAllowedException();
        var video = _videos.FirstOrDefault(v => v.Id == videoId);
        if (video == null) throw new VideoNotFoundException();
        if (Viewers.Any(v => v.VideoId == videoId)) throw new VideoInViewException();
        _videos.Remove(video);
    }
}