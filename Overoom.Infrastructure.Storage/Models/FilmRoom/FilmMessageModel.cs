namespace Overoom.Infrastructure.Storage.Models.FilmRoom;

public class FilmMessageModel
{
    public long Id { get; set; }
    public int ViewerEntityId { get; set; }
    public long ViewerId { get; set; }
    public FilmViewerModel Viewer { get; set; } = null!;

    public Guid RoomId { get; set; }
    public FilmRoomModel Room { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public string Text { get; set; } = null!;
}