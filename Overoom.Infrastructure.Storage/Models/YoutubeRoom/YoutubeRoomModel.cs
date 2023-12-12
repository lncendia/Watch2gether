using System.ComponentModel.DataAnnotations.Schema;
using Overoom.Infrastructure.Storage.Models.Abstractions;

namespace Overoom.Infrastructure.Storage.Models.YoutubeRoom;

public class YoutubeRoomModel : IAggregateModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    public bool IsOpen { get; set; }
    public int IdCounter { get; set; }
    public List<YoutubeMessageModel> Messages { get; set; } = new();
    public List<YoutubeViewerModel> Viewers { get; set; } = new();
    public int OwnerId { get; set; }
    public DateTime LastActivity { get; set; }
    public List<VideoIdModel> VideoIds { get; set; } = new();
    public bool Access { get; set; }
}