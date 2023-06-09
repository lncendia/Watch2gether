using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Overoom.Infrastructure.Storage.Models.Abstractions;
using Overoom.Infrastructure.Storage.Models.Films;

namespace Overoom.Infrastructure.Storage.Models.Playlists;

public class PlaylistModel : IAggregateModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<FilmModel> Films { get; set; } = new();
    public DateTime Updated { get; set; }
    public string PosterUri { get; set; } = null!;
}