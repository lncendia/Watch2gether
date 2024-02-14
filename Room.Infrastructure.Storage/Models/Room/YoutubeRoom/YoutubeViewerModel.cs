using System.ComponentModel.DataAnnotations;
using Room.Infrastructure.Storage.Models.Room.Base;

namespace Room.Infrastructure.Storage.Models.Room.YoutubeRoom;

public class YoutubeViewerModel : ViewerModel<YoutubeRoomModel>
{
    [MaxLength(50)] public string VideoId { get; set; } = null!;
}