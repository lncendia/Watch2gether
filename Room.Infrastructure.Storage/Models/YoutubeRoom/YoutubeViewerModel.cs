using System.ComponentModel.DataAnnotations;
using Room.Infrastructure.Storage.Models.BaseRoom;

namespace Room.Infrastructure.Storage.Models.YoutubeRoom;

public class YoutubeViewerModel : ViewerModel<YoutubeRoomModel>
{
    [MaxLength(50)] public string? VideoId { get; set; }
}