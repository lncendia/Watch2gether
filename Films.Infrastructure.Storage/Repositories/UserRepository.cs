using Microsoft.EntityFrameworkCore;
using Films.Domain.Abstractions.Repositories;
using Films.Domain.Ordering.Abstractions;
using Films.Domain.Specifications.Abstractions;
using Films.Domain.Users;
using Films.Domain.Users.Ordering.Visitor;
using Films.Domain.Users.Specifications.Visitor;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Extensions;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Users;
using Films.Infrastructure.Storage.Visitors.Sorting;
using Films.Infrastructure.Storage.Visitors.Specifications;

namespace Films.Infrastructure.Storage.Repositories;

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
        var user = await context.Users
            .LoadDependencies()
            .AsNoTracking()
            .FirstOrDefaultAsync(userModel => userModel.Id == id);

        if (user == null) return null;
        return await aggregateMapper.MapAsync(user);
    }

    public async Task<IReadOnlyCollection<User>> FindAsync(
        ISpecification<User, IUserSpecificationVisitor>? specification,
        IOrderBy<User, IUserSortingVisitor>? orderBy = null, int? skip = null, int? take = null)
    {
        var query = context.Users.AsQueryable();
        if (specification != null)
        {
            var visitor = new UserVisitor();
            specification.Accept(visitor);
            if (visitor.Expr != null) query = query.Where(visitor.Expr);
        }

        if (orderBy != null)
        {
            var visitor = new UserSortingVisitor();
            orderBy.Accept(visitor);
            var firstQuery = visitor.SortItems.First();
            var orderedQuery = firstQuery.IsDescending
                ? query.OrderByDescending(firstQuery.Expr)
                : query.OrderBy(firstQuery.Expr);

            orderedQuery = visitor.SortItems.Skip(1)
                .Aggregate(orderedQuery, (current, sort) => sort.IsDescending
                    ? current.ThenByDescending(sort.Expr)
                    : current.ThenBy(sort.Expr));

            query = orderedQuery.ThenBy(v => v.Id);
        }
        else
        {
            query = query.OrderBy(x => x.Id);
        }

        if (skip.HasValue) query = query.Skip(skip.Value);
        if (take.HasValue) query = query.Take(take.Value);

        var entities = await query
            .LoadDependencies()
            .AsNoTracking()
            .ToArrayAsync();

        var aggregates = new User[entities.Length];
        for (var i = 0; i < aggregates.Length; i++) aggregates[i] = await aggregateMapper.MapAsync(entities[i]);
        return aggregates;
    }

    public Task<int> CountAsync(ISpecification<User, IUserSpecificationVisitor>? specification)
    {
        var query = context.Users.AsQueryable();
        if (specification == null) return query.CountAsync();
        var visitor = new UserVisitor();
        specification.Accept(visitor);
        if (visitor.Expr != null) query = query.Where(visitor.Expr);

        return query.CountAsync();
    }
}