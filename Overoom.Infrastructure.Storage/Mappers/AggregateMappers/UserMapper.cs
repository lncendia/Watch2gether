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

    private static readonly FieldInfo UserProxy =
        typeof(User).GetField("<ProxyId>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;
    

    public User Map(UserModel model)
    {
        var user = new User(model.Name, model.Email);
        IdFields.AggregateId.SetValue(user, model.Id);
        if (model.Target.HasValue)
        {
            user.SetTarget(model.Target.Value);
            TargetDate.SetValue(user.Target, model.TargetSetTime);
        }
        
        UserProxy.SetValue(user, model.ProxyId);
        if (model.SubscriptionDate.HasValue)
            UserSubscription.SetValue(user, GetSubscription(model.SubscriptionDate.Value, model.ExpirationDate!.Value));
        if (model.AccessToken != null) user.SetVk(model.VkName!, model.AccessToken);
        return user;
    }
}