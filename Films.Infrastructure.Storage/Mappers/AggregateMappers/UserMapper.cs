using System.Reflection;
using Films.Domain.Users;
using Films.Domain.Users.ValueObjects;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Users;

namespace Films.Infrastructure.Storage.Mappers.AggregateMappers;

internal class UserMapper : IAggregateMapperUnit<User, UserModel>
{
    private static readonly Type UserType = typeof(User);
    private static readonly Type FilmNoteType = typeof(FilmNote);

    private static readonly FieldInfo WatchList =
        UserType.GetField("_watchlist", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo History =
        UserType.GetField("_history", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo Genres =
        UserType.GetField("_genres", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo Date =
        FilmNoteType.GetField("<Date>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;


    public Task<User> MapAsync(UserModel model)
    {
        var user = new User(model.Id)
        {
            Username = model.UserName,
            PhotoUrl = model.PhotoUrl,
            Allows = new Allows
            {
                Beep = model.Beep,
                Scream = model.Scream,
                Change = model.Change
            }
        };

        var watchlist = model.Watchlist
            .Select(x =>
            {
                var note = new FilmNote
                {
                    FilmId = x.FilmId
                };
                Date.SetValue(note, x.Date);
                return note;
            })
            .ToList();

        var history = model.History
            .Select(x =>
            {
                var note = new FilmNote
                {
                    FilmId = x.FilmId
                };
                Date.SetValue(note, x.Date);
                return note;
            })
            .ToList();

        var genres = model.Genres.Select(x => x.Name).ToList();

        WatchList.SetValue(user, watchlist);
        History.SetValue(user, history);
        Genres.SetValue(user, genres);

        return Task.FromResult(user);
    }
}