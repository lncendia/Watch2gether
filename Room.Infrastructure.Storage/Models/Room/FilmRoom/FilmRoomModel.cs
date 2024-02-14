using System.ComponentModel.DataAnnotations;
using Room.Infrastructure.Storage.Models.Film;
using Room.Infrastructure.Storage.Models.Room.Base;

namespace Room.Infrastructure.Storage.Models.Room.FilmRoom;

public class FilmRoomModel : RoomModel
{
    public Guid FilmId { get; set; }
    public FilmModel Film { get; set; } = null!;
    [MaxLength(30)] public string CdnName { get; set; } = null!;
    public List<FilmViewerModel> Viewers { get; set; } = [];
    public List<BannedModel<FilmRoomModel>> BannedUsers { get; set; } = [];
    
    public List<MessageModel<FilmRoomModel>> Messages { get; set; } = [];
}