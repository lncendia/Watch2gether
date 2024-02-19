using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Room.Infrastructure.Storage.Models.YoutubeRoom;

[PrimaryKey(nameof(RoomId), nameof(VideoId))]
public class VideoModel
{
    public Guid RoomId { get; set; }
    public YoutubeRoomModel Room { get; set; } = null!;

    public DateTime Added { get; set; }

    [MaxLength(50)] public string VideoId { get; set; } = null!;
}