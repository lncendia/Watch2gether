using System.Reflection;
using Films.Domain.Abstractions;
using Films.Domain.Users.Entities;
using Films.Domain.Users.ValueObjects;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Mappers.StaticMethods;
using Films.Infrastructure.Storage.Models.User;

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


    public User Map(UserModel model)
    {
        var user = new User
        {
            UserName = model.UserName,
            PhotoUrl = model.PhotoUrl
        };
        user.UpdateAllows(model.Beep, model.Scream, model.Change);
        IdFields.AggregateId.SetValue(user, model.Id);

        object?[] args = new object[1];
        var watchlist = model.Watchlist.Select(x =>
        {
            args[0] = x.FilmId;
            var element = (FilmNote)FilmNoteType.Assembly.CreateInstance(
                FilmNoteType.FullName!, false, BindingFlags.Instance | BindingFlags.NonPublic, null, args!,
                null, null)!;
            Date.SetValue(element, x.Date);
            return element;
        }).ToList();
        var history = model.History.Select(x =>
        {
            args[0] = x.FilmId;
            var element = (FilmNote)FilmNoteType.Assembly.CreateInstance(
                FilmNoteType.FullName!, false, BindingFlags.Instance | BindingFlags.NonPublic, null, args!,
                null, null)!;
            Date.SetValue(element, x.Date);
            return element;
        }).ToList();

        var genres = model.Genres.Select(x => x.Name).ToList();

        WatchList.SetValue(user, watchlist);
        History.SetValue(user, history);
        Genres.SetValue(user, genres);

        var domainCollection = (List<IDomainEvent>)IdFields.DomainEvents.GetValue(user)!;
        domainCollection.Clear();
        return user;
    }
}