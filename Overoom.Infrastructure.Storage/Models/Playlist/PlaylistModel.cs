using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Overoom.Infrastructure.Storage.Models.Abstractions;
using Overoom.Infrastructure.Storage.Models.Genre;

namespace Overoom.Infrastructure.Storage.Models.Playlist;

public class PlaylistModel : IAggregateModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    [MaxLength(200)] public string Name { get; set; } = null!;
    [MaxLength(500)]public string Description { get; set; } = null!;
    public List<PlaylistFilmModel> Films { get; set; } = new();
    public List<GenreModel> Genres { get; set; } = new();
    public DateTime Updated { get; set; }
    public Uri PosterUri { get; set; } = null!;
}