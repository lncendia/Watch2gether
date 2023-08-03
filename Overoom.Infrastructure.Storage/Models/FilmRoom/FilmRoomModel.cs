using Overoom.Domain.Films.Enums;
using Overoom.Infrastructure.Storage.Models.Abstractions;
using Overoom.Infrastructure.Storage.Models.Film;

namespace Overoom.Infrastructure.Storage.Models.FilmRoom;

public class FilmRoomModel : IAggregateModel
{
    public Guid FilmId { get; set; }
    public FilmModel Film { get; set; } = null!;
    public CdnType CdnType { get; set; }
    public Guid Id { get; set; }
    public bool IsOpen { get; set; }
    public int IdCounter { get; set; }
    public List<FilmMessageModel> Messages { get; set; } = new();
    public List<FilmViewerModel> Viewers { get; set; } = new();
    public int OwnerId { get; set; }
    public DateTime LastActivity { get; set; }
}