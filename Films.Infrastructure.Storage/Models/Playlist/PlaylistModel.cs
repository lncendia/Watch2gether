using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Films.Infrastructure.Storage.Models.Abstractions;
using Films.Infrastructure.Storage.Models.Genre;
using Films.Infrastructure.Storage.Models.Film;

namespace Films.Infrastructure.Storage.Models.Playlist;

public class PlaylistModel : IAggregateModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    [MaxLength(200)] public string Name { get; set; } = null!;
    [MaxLength(500)]public string Description { get; set; } = null!;
    public List<PlaylistFilmModel> Films { get; set; } = [];
    public List<GenreModel> Genres { get; set; } = [];
    public DateTime Updated { get; set; }
    public Uri PosterUrl { get; set; } = null!;
}