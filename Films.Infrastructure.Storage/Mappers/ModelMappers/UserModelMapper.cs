using Films.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Extensions;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.User;

namespace Films.Infrastructure.Storage.Mappers.ModelMappers;

internal class UserModelMapper(ApplicationDbContext context) : IModelMapperUnit<UserModel, User>
{
    public async Task<UserModel> MapAsync(User aggregate)
    {
        var model = await context.Users
            .LoadDependencies()
            .FirstOrDefaultAsync(x => x.Id == aggregate.Id) ?? new UserModel { Id = aggregate.Id };

        model.UserName = aggregate.UserName;
        model.PhotoUrl = aggregate.PhotoUrl;
        model.Beep = aggregate.Allows.Beep;
        model.Scream = aggregate.Allows.Scream;
        model.Change = aggregate.Allows.Change;


        ProcessHistory(aggregate, model);

        ProcessWatchlist(aggregate, model);

        await ProcessGenresAsync(aggregate, model);

        return model;
    }

    private static void ProcessHistory(User aggregate, UserModel model)
    {
        model.History.RemoveAll(uh => aggregate.History.All(eh => eh.FilmId != uh.FilmId && eh.Date != uh.Date));

        // Нужно добавить в UserHistory новые записи, которых там еще нет
        var historyToAdd = aggregate.History
            .Where(eh => model.History.All(uh => uh.FilmId != eh.FilmId && uh.Date != eh.Date))
            .Select(eh => new HistoryModel { FilmId = eh.FilmId, Date = eh.Date });

        model.History.AddRange(historyToAdd);
    }

    private static void ProcessWatchlist(User aggregate, UserModel model)
    {
        model.Watchlist.RemoveAll(uw => aggregate.Watchlist.All(eh => eh.FilmId != uw.FilmId && eh.Date != uw.Date));

        // Нужно добавить в UserWatchlist новые записи, которых там еще нет
        var watchlistToAdd = aggregate.Watchlist
            .Where(eh => model.Watchlist.All(uh => uh.FilmId != eh.FilmId && uh.Date != eh.Date))
            .Select(eh => new WatchlistModel { FilmId = eh.FilmId, Date = eh.Date });

        model.Watchlist.AddRange(watchlistToAdd);
    }

    private async Task ProcessGenresAsync(User aggregate, UserModel model)
    {
        model.Genres.RemoveAll(x => aggregate.Genres.All(m => m != x.Name));

        var newGenres = aggregate.Genres
            .Where(x => model.Genres.All(m => m.Name != x))
            .ToArray();

        if (newGenres.Length == 0) return;

        var databaseGenres = await context.Genres
            .Where(x => newGenres.Any(g => g == x.Name))
            .ToArrayAsync();

        model.Genres.AddRange(databaseGenres);
    }
}