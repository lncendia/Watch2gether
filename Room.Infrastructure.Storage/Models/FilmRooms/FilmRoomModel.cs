using System.ComponentModel.DataAnnotations;
using Room.Infrastructure.Storage.Models.Rooms;

namespace Room.Infrastructure.Storage.Models.FilmRooms;

public class FilmRoomModel : RoomModel
{
    /// <summary> 
    /// Заголовок фильма. 
    /// </summary> 
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    [MaxLength(30)] public string CdnName { get; set; } = null!;
    public Uri CdnUrl { get; set; } = null!;

    public bool IsSerial { get; set; }
    public List<FilmViewerModel> Viewers { get; set; } = [];
}