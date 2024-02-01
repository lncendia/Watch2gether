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
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == entity.Id);
        if (user != null)
        {
            await context.Entry(user).Collection(x => x.History).LoadAsync();
            await context.Entry(user).Collection(x => x.Watchlist).LoadAsync();
            await context.Entry(user).Collection(x => x.Genres).LoadAsync();
        }
        else user = new UserModel { Id = entity.Id };

        user.UserName = entity.UserName;
        user.PhotoUrl = entity.PhotoUrl;
        user.Beep = entity.Allows.Beep;
        user.Scream = entity.Allows.Scream;
        user.Change = entity.Allows.Change;


        #region History

        // Нужно удалить из UserHistory записи, которых нет в entity.History
        var historyToRemove =
            user.History.Where(uh => !entity.History.Any(eh => eh.FilmId == uh.FilmId && eh.Date == uh.Date));

        context.UserHistory.RemoveRange(historyToRemove);

        // Нужно добавить в UserHistory новые записи, которых там еще нет
        var historyToAdd = entity.History
            .Where(eh => !user.History.Any(uh => uh.FilmId == eh.FilmId && uh.Date == eh.Date))
            .Select(eh => new HistoryModel { FilmId = eh.FilmId, Date = eh.Date });

        user.History.AddRange(historyToAdd);

        #endregion

        #region Watchlist

        // Нужно удалить из UserWatchlist записи, которых нет в entity.Watchlist
        var watchlistToRemove =
            user.Watchlist.Where(uh => !entity.Watchlist.Any(eh => eh.FilmId == uh.FilmId && eh.Date == uh.Date));

        context.UserWatchlist.RemoveRange(watchlistToRemove);

        // Нужно добавить в UserWatchlist новые записи, которых там еще нет
        var watchlistToAdd = entity.Watchlist
            .Where(eh => !user.Watchlist.Any(uh => uh.FilmId == eh.FilmId && uh.Date == eh.Date))
            .Select(eh => new WatchlistModel { FilmId = eh.FilmId, Date = eh.Date });

        user.Watchlist.AddRange(watchlistToAdd);

        #endregion


        var newGenres = entity.Genres.Where(x => user.Genres.All(m => !string.Equals(m.Name, x, StringComparison.CurrentCultureIgnoreCase)));
        var removeGenres = user.Genres.Where(x => entity.Genres.All(m => !string.Equals(m, x.Name, StringComparison.CurrentCultureIgnoreCase)));
        var databaseGenres = context.Genres.Where(x => newGenres.Any(g => g.Equals(x.Name, StringComparison.CurrentCultureIgnoreCase)));
        user.Genres.AddRange(databaseGenres);
        user.Genres.RemoveAll(g => removeGenres.Contains(g));

        return user;
    }
}