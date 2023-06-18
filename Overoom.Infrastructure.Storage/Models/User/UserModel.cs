using Overoom.Infrastructure.Storage.Models.Abstractions;

namespace Overoom.Infrastructure.Storage.Models.User;

public class UserModel : IAggregateModel
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Uri AvatarUri { get; set; } = null!;
    public List<WatchlistModel> Watchlist { get; set; } = new();
    public List<HistoryModel> History { get; set; } = new();
}