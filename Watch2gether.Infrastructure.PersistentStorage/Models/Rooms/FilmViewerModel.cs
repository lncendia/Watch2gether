using Watch2gether.Infrastructure.PersistentStorage.Models.Rooms.Base;

namespace Watch2gether.Infrastructure.PersistentStorage.Models.Rooms;

public class FilmViewerModel : ViewerBaseModel
{
    public int Season { get; set; }
    public int Series { get; set; }
}