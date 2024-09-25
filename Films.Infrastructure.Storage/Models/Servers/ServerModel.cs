using System.ComponentModel.DataAnnotations.Schema;

using Films.Infrastructure.Storage.Models.Abstractions;
using Films.Infrastructure.Storage.Models.FilmRooms;
using Films.Infrastructure.Storage.Models.YoutubeRoom;

namespace Films.Infrastructure.Storage.Models.Servers;

public class ServerModel : IAggregateModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    public int MaxRoomsCount { get; set; }
    public bool IsEnabled { get; set; }
    public Uri Url { get; set; } = null!;
    
    public List<FilmRoomModel>? FilmRooms { get; set; }
    public List<YoutubeRoomModel>? YoutubeRooms { get; set; }
}