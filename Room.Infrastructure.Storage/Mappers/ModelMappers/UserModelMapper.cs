using Microsoft.EntityFrameworkCore;
using Room.Domain.Users.Entities;
using Room.Infrastructure.Storage.Context;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Models.User;

namespace Room.Infrastructure.Storage.Mappers.ModelMappers;

internal class UserModelMapper(ApplicationDbContext context) : IModelMapperUnit<UserModel, User>
{
    public async Task<UserModel> MapAsync(User entity)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == entity.Id) ?? new UserModel { Id = entity.Id };

        user.UserName = entity.UserName;
        user.PhotoUrl = entity.PhotoUrl;
        user.Beep = entity.Allows.Beep;
        user.Scream = entity.Allows.Scream;
        user.Change = entity.Allows.Change;

        return user;
    }
}