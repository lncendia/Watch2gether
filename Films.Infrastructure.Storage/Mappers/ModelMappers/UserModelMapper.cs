using Microsoft.EntityFrameworkCore;
using Films.Domain.Users.Entities;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Extensions;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.User;

namespace Films.Infrastructure.Storage.Mappers.ModelMappers;

internal class UserModelMapper(ApplicationDbContext context) : IModelMapperUnit<UserModel, User>
{
    public async Task<UserModel> MapAsync(User entity)
    {
        var user = await context.Users
            .LoadDependencies()
            .FirstOrDefaultAsync(x => x.Id == entity.Id) ?? new UserModel { Id = entity.Id };

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
        user.History.RemoveAll(uh => entity.History.All(eh => eh.FilmId != uh.FilmId && eh.Date != uh.Date));

        // Нужно добавить в UserHistory новые записи, которых там еще нет
        var historyToAdd = entity.History
            .Where(eh => user.History.All(uh => uh.FilmId != eh.FilmId && uh.Date != eh.Date))
            .Select(eh => new HistoryModel { FilmId = eh.FilmId, Date = eh.Date });

        user.History.AddRange(historyToAdd);
    }

    private static void ProcessWatchlist(User entity, UserModel user)
    {
        user.Watchlist.RemoveAll(uw => entity.Watchlist.All(eh => eh.FilmId != uw.FilmId && eh.Date != uw.Date));

        // Нужно добавить в UserWatchlist новые записи, которых там еще нет
        var watchlistToAdd = entity.Watchlist
            .Where(eh => user.Watchlist.All(uh => uh.FilmId != eh.FilmId && uh.Date != eh.Date))
            .Select(eh => new WatchlistModel { FilmId = eh.FilmId, Date = eh.Date });

        user.Watchlist.AddRange(watchlistToAdd);
    }

    private async Task ProcessGenresAsync(User entity, UserModel user)
    {
        user.Genres.RemoveAll(x => entity.Genres.All(m => m != x.Name));

        var newGenres = entity.Genres
            .Where(x => user.Genres.All(m => m.Name != x))
            .ToArray();

        if (newGenres.Length == 0) return;

        var databaseGenres = await context.Genres
            .Where(x => newGenres.Any(g => g == x.Name))
            .ToArrayAsync();

        user.Genres.AddRange(databaseGenres);
    }
}