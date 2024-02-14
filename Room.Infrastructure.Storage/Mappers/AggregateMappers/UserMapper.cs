using Room.Domain.Users.Entities;
using Room.Domain.Users.ValueObjects;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Models.User;

namespace Room.Infrastructure.Storage.Mappers.AggregateMappers;

internal class UserMapper : IAggregateMapperUnit<User, UserModel>
{
    public User Map(UserModel model)
    {
        var user = new User(model.Id)
        {
            UserName = model.UserName,
            PhotoUrl = model.PhotoUrl,
            Allows = new Allows
            {
                Beep = model.Beep,
                Scream = model.Scream,
                Change = model.Change
            }
        };
        
        return user;
    }
}