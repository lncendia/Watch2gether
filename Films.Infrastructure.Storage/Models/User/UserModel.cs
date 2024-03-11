using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Films.Infrastructure.Storage.Models.Abstractions;
using Films.Infrastructure.Storage.Models.Genre;

namespace Films.Infrastructure.Storage.Models.User;

public class UserModel : IAggregateModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    [MaxLength(40)] public string UserName { get; set; } = null!;
    public Uri? PhotoUrl { get; set; }
    public List<WatchlistModel> Watchlist { get; set; } = [];
    public List<HistoryModel> History { get; set; } = [];
    public List<GenreModel> Genres { get; set; } = [];


    public bool Beep { get; set; }
    public bool Scream { get; set; }
    public bool Change { get; set; }
}