using System.Reflection;
using Overoom.Domain.User.Entities;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;
using Overoom.Infrastructure.Storage.Models.User;

namespace Overoom.Infrastructure.Storage.Mappers.AggregateMappers;

internal class UserMapper : IAggregateMapperUnit<User, UserModel>
{
    private static readonly FieldInfo WatchList =
        typeof(User).GetField("_watchlist", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo History =
        typeof(User).GetField("_history", BindingFlags.Instance | BindingFlags.NonPublic)!;


    public User Map(UserModel model)
    {
        var user = new User(model.Name, model.Email, model.AvatarUri);
        IdFields.AggregateId.SetValue(user, model.Id);
        WatchList.SetValue(user, model.Watchlist.Select(x => x.FilmId).ToList());
        History.SetValue(user, model.History.Select(x => x.FilmId).ToList());
        return user;
    }
}