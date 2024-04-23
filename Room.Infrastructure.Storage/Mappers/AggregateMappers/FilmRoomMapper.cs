using System.Reflection;
using Room.Domain.Rooms.FilmRooms;
using Room.Domain.Rooms.FilmRooms.Entities;
using Room.Domain.Rooms.FilmRooms.ValueObjects;
using Room.Domain.Rooms.Rooms;
using Room.Domain.Rooms.Rooms.Entities;
using Room.Domain.Rooms.Rooms.ValueObjects;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Models.FilmRooms;

namespace Room.Infrastructure.Storage.Mappers.AggregateMappers;

internal class FilmRoomMapper : IAggregateMapperUnit<FilmRoom, FilmRoomModel>
{
    private static readonly Type FilmViewerType = typeof(FilmViewer);
    private static readonly Type ViewerType = typeof(Viewer);
    private static readonly Type RoomType = typeof(Room<FilmViewer>);
    

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

    private static readonly FieldInfo ViewerSeason =
        FilmViewerType.GetField("<Season>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo ViewerSeries =
        FilmViewerType.GetField("<Series>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public FilmRoom Map(FilmRoomModel model)
    {
        var viewers = model.Viewers.Select(CreateViewer).ToList();
        var owner = viewers.First(v => v.Id == model.Viewers.First(m => m.Owner).Id);

        var room = new FilmRoom
        {
            Id = model.Id,
            Cdn = new Cdn
            {
                Name = model.CdnName,
                Url = model.CdnUrl
            },
            IsSerial = model.IsSerial,
            Title = model.Title,
            Owner = owner
        };
        
        ViewersList.SetValue(room, viewers);
        return room;
    }

    private static FilmViewer CreateViewer(FilmViewerModel model)
    {
        var viewer = new FilmViewer
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
        ViewerSeason.SetValue(viewer, model.Season);
        ViewerSeries.SetValue(viewer, model.Series);
        
        return viewer;
    }
}