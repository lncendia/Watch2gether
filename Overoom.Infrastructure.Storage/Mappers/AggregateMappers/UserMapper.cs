using System.Reflection;
using Overoom.Domain.User.Entities;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;
using Overoom.Infrastructure.Storage.Models.Users;

namespace Overoom.Infrastructure.Storage.Mappers.AggregateMappers;

internal class UserMapper : IAggregateMapperUnit<User, UserModel>
{
    private static readonly FieldInfo UserSubscription =
        typeof(User).GetField("<Subscription>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;
    //todo: favorite and watched films
    

    public User Map(UserModel model)
    {
        var user = new User(model.Name, model.Email, model.AvatarUri);
        IdFields.AggregateId.SetValue(user, model.Id);
        return user;
    }
}