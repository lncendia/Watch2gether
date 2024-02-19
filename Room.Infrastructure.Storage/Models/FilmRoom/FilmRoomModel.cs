using System.ComponentModel.DataAnnotations;
using Room.Infrastructure.Storage.Models.BaseRoom;

namespace Room.Infrastructure.Storage.Models.FilmRoom;

public class FilmRoomModel : RoomModel
{
    /// <summary> 
    /// Заголовок фильма. 
    /// </summary> 
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    public Uri CdnUrl { get; set; } = null!;

    public bool IsSerial { get; set; }
    public List<FilmViewerModel> Viewers { get; set; } = [];
    public List<MessageModel<FilmRoomModel>> Messages { get; set; } = [];
}