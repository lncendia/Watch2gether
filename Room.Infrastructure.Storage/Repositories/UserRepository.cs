using Microsoft.EntityFrameworkCore;
using Room.Domain.Abstractions.Repositories;
using Room.Domain.Users.Entities;
using Room.Infrastructure.Storage.Context;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Models.User;

namespace Room.Infrastructure.Storage.Repositories;

public class UserRepository(
    ApplicationDbContext context,
    IAggregateMapperUnit<User, UserModel> aggregateMapper,
    IModelMapperUnit<UserModel, User> modelMapper)
    : IUserRepository
{
    public async Task AddAsync(User entity)
    {
        var user = await modelMapper.MapAsync(entity);
        context.Notifications.AddRange(entity.DomainEvents);
        await context.AddAsync(user);
    }

    public async Task UpdateAsync(User entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        await modelMapper.MapAsync(entity);
    }

    public Task DeleteAsync(Guid id)
    {
        context.Remove(context.Users.First(user => user.Id == id));
        return Task.CompletedTask;
    }

    public async Task<User?> GetAsync(Guid id)
    {
        var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(userModel => userModel.Id == id);
        return user == null ? null : aggregateMapper.Map(user);
    }
}