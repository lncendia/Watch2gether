using System.Reflection;
using System.Runtime.CompilerServices;
using Room.Domain.Rooms.Rooms;
using Room.Domain.Rooms.Rooms.Entities;
using Room.Domain.Rooms.Rooms.ValueObjects;
using Room.Domain.Rooms.YoutubeRooms;
using Room.Domain.Rooms.YoutubeRooms.Entities;
using Room.Domain.Rooms.YoutubeRooms.ValueObjects;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Models.YoutubeRooms;

namespace Room.Infrastructure.Storage.Mappers.AggregateMappers;

internal class YoutubeRoomMapper : IAggregateMapperUnit<YoutubeRoom, YoutubeRoomModel>
{
    private static readonly Type YoutubeViewerType = typeof(YoutubeViewer);
    private static readonly Type ViewerType = typeof(Viewer);

    private static readonly Type YoutubeRoomType = typeof(YoutubeRoom);
    private static readonly Type RoomType = typeof(Room<YoutubeViewer>);

    private static readonly Type VideoType = typeof(Video);

    private static readonly FieldInfo Videos =
        YoutubeRoomType.GetField("_videos", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo ViewersList =
        RoomType.GetField("_viewersList", BindingFlags.Instance | BindingFlags.NonPublic)!;
    
    private static readonly FieldInfo ViewerOnline =
        ViewerType.GetField("<Online>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo ViewerFullScreen =
        ViewerType.GetField("<FullScreen>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo ViewerPause =
        ViewerType.GetField("<Pause>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo ViewerTimeLine =
        ViewerType.GetField("<TimeLine>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo ViewerVideoId =
        YoutubeViewerType.GetField("<VideoId>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo VideoId =
        VideoType.GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo VideoAdded =
        VideoType.GetField("<Added>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public YoutubeRoom Map(YoutubeRoomModel model)
    {
        var videos = model.Videos.Select(CreateVideo).ToList();
        var viewers = model.Viewers.Select(CreateViewer).ToList();
        var owner = viewers.First(v => v.Id == model.Viewers.First(m => m.Owner).Id);

        var room = new YoutubeRoom
        {
            Id = model.Id,
            Owner = owner,
            VideoAccess = model.VideoAccess
        };
        
        ViewersList.SetValue(room, viewers);
        Videos.SetValue(room, videos);
        return room;
    }

    private static YoutubeViewer CreateViewer(YoutubeViewerModel model)
    {
        var viewer = new YoutubeViewer
        {
            Id = model.Id,
            Username = model.Username,
            PhotoUrl = model.PhotoUrl,
            Allows = new Allows
            {
                Beep = model.Beep,
                Scream = model.Scream,
                Change = model.Change
            }
        };
        ViewerFullScreen.SetValue(viewer, model.FullScreen);
        ViewerOnline.SetValue(viewer, model.Online);
        ViewerPause.SetValue(viewer, model.Pause);
        ViewerTimeLine.SetValue(viewer, model.TimeLine);
        ViewerVideoId.SetValue(viewer, model.VideoId);

        return viewer;
    }
    

    private static Video CreateVideo(VideoModel model)
    {
        var video = (Video)RuntimeHelpers.GetUninitializedObject(VideoType);
        VideoId.SetValue(video, model.VideoId);
        VideoAdded.SetValue(video, model.Added);
        return video;
    }
}