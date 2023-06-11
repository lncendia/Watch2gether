using Microsoft.EntityFrameworkCore;
using Overoom.Domain.User.Entities;
using Overoom.Infrastructure.Storage.Context;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Models.User;

namespace Overoom.Infrastructure.Storage.Mappers.ModelMappers;

internal class UserModelMapper : IModelMapperUnit<UserModel, User>
{
    private readonly ApplicationDbContext _context;

    public UserModelMapper(ApplicationDbContext context) => _context = context;

    public async Task<UserModel> MapAsync(User entity)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == entity.Id);
        if (user != null)
        {
            await _context.Entry(user).Collection(x => x.History).LoadAsync();
            await _context.Entry(user).Collection(x => x.Watchlist).LoadAsync();
        }
        else user = new UserModel { Id = entity.Id };

        user.Name = entity.Name;
        user.Email = entity.Email;
        user.AvatarUri = entity.AvatarUri;


        var newHistory = entity.History.Where(x => user.History.All(m => m.FilmId != x));
        user.History.AddRange(newHistory.Select(x => new HistoryModel { FilmId = x }));
        user.History.RemoveAll(x => entity.History.All(m => m != x.FilmId));

        var newWatchlist = entity.Watchlist.Where(x => user.Watchlist.All(m => m.FilmId != x));
        user.Watchlist.AddRange(newWatchlist.Select(x => new WatchlistModel { FilmId = x }));
        user.Watchlist.RemoveAll(x => entity.Watchlist.All(m => m != x.FilmId));

        return user;
    }
}