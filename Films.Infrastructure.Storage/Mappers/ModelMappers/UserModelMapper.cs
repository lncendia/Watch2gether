using Microsoft.EntityFrameworkCore;
using Films.Domain.Users.Entities;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.User;
// ReSharper disable EntityFramework.NPlusOne.IncompleteDataUsage
// ReSharper disable EntityFramework.NPlusOne.IncompleteDataQuery

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


        ProcessHistory(entity, user);

        ProcessWatchlist(entity, user);

        await ProcessGenresAsync(entity, user);

        return user;
    }

    private static void ProcessHistory(User entity, UserModel user)
    {
        // Нужно удалить из UserHistory записи, которых нет в entity.History
        var historyToRemove =
            user.History.Where(uh => !entity.History.Any(eh => eh.FilmId == uh.FilmId && eh.Date == uh.Date));

        user.History.RemoveAll(g => historyToRemove.Contains(g));

        // Нужно добавить в UserHistory новые записи, которых там еще нет
        var historyToAdd = entity.History
            .Where(eh => !user.History.Any(uh => uh.FilmId == eh.FilmId && uh.Date == eh.Date))
            .Select(eh => new HistoryModel { FilmId = eh.FilmId, Date = eh.Date });

        user.History.AddRange(historyToAdd);
    }
    
    private static void ProcessWatchlist(User entity, UserModel user)
    {
        // Нужно удалить из UserWatchlist записи, которых нет в entity.Watchlist
        var watchlistToRemove =
            user.Watchlist.Where(uh => !entity.Watchlist.Any(eh => eh.FilmId == uh.FilmId && eh.Date == uh.Date));

        user.Watchlist.RemoveAll(g => watchlistToRemove.Contains(g));

        // Нужно добавить в UserWatchlist новые записи, которых там еще нет
        var watchlistToAdd = entity.Watchlist
            .Where(eh => !user.Watchlist.Any(uh => uh.FilmId == eh.FilmId && uh.Date == eh.Date))
            .Select(eh => new WatchlistModel { FilmId = eh.FilmId, Date = eh.Date });

        user.Watchlist.AddRange(watchlistToAdd);
    }
    
    private async Task ProcessGenresAsync(User entity, UserModel user)
    {
        var removeGenres = user.Genres
            .Where(x => entity.Genres.All(m => m != x.Name));
        
        user.Genres.RemoveAll(g => removeGenres.Contains(g));
        
        var newGenres = entity.Genres
            .Where(x => user.Genres.All(m => m.Name != x))
            .ToArray();
        
        if (newGenres.Length != 0)
        {
            var databaseGenres = await context.Genres
                .Where(x => newGenres.Any(g => g == x.Name))
                .ToArrayAsync();
            
            user.Genres.AddRange(databaseGenres);
        }
    }
}