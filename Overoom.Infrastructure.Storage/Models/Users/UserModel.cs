using Overoom.Infrastructure.Storage.Models.Abstractions;

namespace Overoom.Infrastructure.Storage.Models.Users;

public class UserModel : IAggregateModel
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string AvatarUri { get; set; } = null!;
    public List<WatchlistModel> Watchlist { get; set; } = new();
    public List<HistoryModel> History { get; set; } = new();
}