using Microsoft.EntityFrameworkCore;
using Films.Domain.Users.Entities;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.User;

namespace Films.Infrastructure.Storage.Mappers.ModelMappers;

internal class UserModelMapper(ApplicationDbContext context) : IModelMapperUnit<UserModel, User>
{
    public async Task<UserModel> MapAsync(User entity)
    {
        var user = context.Users.Local.FirstOrDefault(x => x.Id == entity.Id);
        if (user == null)
        {
            user = await context.Users.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (user != null)
            {
                await context.Entry(user).Collection(x => x.History).LoadAsync();
                await context.Entry(user).Collection(x => x.Watchlist).LoadAsync();
                await context.Entry(user).Collection(x => x.Genres).LoadAsync();
            }
            else user = new UserModel { Id = entity.Id };
        }

        user.UserName = entity.UserName;
        user.PhotoUrl = entity.PhotoUrl;
        user.Beep = entity.Allows.Beep;
        user.Scream = entity.Allows.Scream;
        user.Change = entity.Allows.Change;


        if (entity.History.Count != 0)
        {
            var newHistory = entity.History.Where(x => user.History.All(m => m.FilmId != x.FilmId || m.Date != x.Date));
            user.History.AddRange(newHistory.Select(x => new HistoryModel { FilmId = x.FilmId, Date = x.Date }));
            context.UserHistory.RemoveRange(user.History.Where(x =>
                entity.History.All(m => m.FilmId != x.FilmId || m.Date != x.Date)));
        }

        if (entity.Watchlist.Count != 0)
        {
            var newWatchlist =
                entity.Watchlist.Where(x => user.Watchlist.All(m => m.FilmId != x.FilmId || m.Date != x.Date));
            user.Watchlist.AddRange(newWatchlist.Select(x => new WatchlistModel { FilmId = x.FilmId, Date = x.Date }));
            context.UserWatchlist.RemoveRange(user.Watchlist.Where(x =>
                entity.Watchlist.All(m => m.FilmId != x.FilmId || m.Date != x.Date)));
        }


        if (entity.Genres.Count != 0)
        {
            var newGenres = entity.Genres.Where(x => user.Genres.All(m => !string.Equals(m.Name, x, StringComparison.CurrentCultureIgnoreCase)));
            var removeGenres = user.Genres.Where(x => entity.Genres.All(m => !string.Equals(m, x.Name, StringComparison.CurrentCultureIgnoreCase)));
            var databaseGenres = context.Genres
                .Where(x => newGenres.Select(s => s.ToUpper()).Any(g => g.Equals(x.Name, StringComparison.CurrentCultureIgnoreCase))).ToList();
            user.Genres.AddRange(databaseGenres);
            user.Genres.RemoveAll(g => removeGenres.Contains(g));
        }

        return user;
    }
}