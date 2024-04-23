using System.ComponentModel.DataAnnotations;
using Room.Infrastructure.Storage.Models.Rooms;

namespace Room.Infrastructure.Storage.Models.YoutubeRooms;

public class YoutubeViewerModel : ViewerModel<YoutubeRoomModel>
{
    [MaxLength(50)] public string? VideoId { get; set; }
}