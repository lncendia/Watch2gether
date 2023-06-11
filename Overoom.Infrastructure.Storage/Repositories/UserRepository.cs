using Microsoft.EntityFrameworkCore;
using Overoom.Domain.Abstractions.Repositories;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.User.Entities;
using Overoom.Domain.User.Ordering.Visitor;
using Overoom.Domain.User.Specifications.Visitor;
using Overoom.Infrastructure.Storage.Context;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Models.User;
using Overoom.Infrastructure.Storage.Visitors.Sorting;
using Overoom.Infrastructure.Storage.Visitors.Specifications;

namespace Overoom.Infrastructure.Storage.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IAggregateMapperUnit<User, UserModel> _aggregateMapper;
    private readonly IModelMapperUnit<UserModel, User> _modelMapper;

    public UserRepository(ApplicationDbContext context, IAggregateMapperUnit<User, UserModel> aggregateMapper,
        IModelMapperUnit<UserModel, User> modelMapper)
    {
        _context = context;
        _aggregateMapper = aggregateMapper;
        _modelMapper = modelMapper;
    }

    public async Task AddAsync(User entity)
    {
        var user = await _modelMapper.MapAsync(entity);
        _context.Notifications.AddRange(entity.DomainEvents);
        await _context.AddAsync(user);
    }

    public async Task UpdateAsync(User entity)
    {
        _context.Notifications.AddRange(entity.DomainEvents);
        await _modelMapper.MapAsync(entity);
    }

    public Task DeleteAsync(Guid id)
    {
        _context.Remove(_context.Users.First(user => user.Id == id));
        return Task.CompletedTask;
    }

    public async Task<User?> GetAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(userModel => userModel.Id == id);
        if (user == null) return null;
        await LoadCollectionsAsync(user);
        return _aggregateMapper.Map(user);
    }

    public async Task<IList<User>> FindAsync(ISpecification<User, IUserSpecificationVisitor>? specification,
        IOrderBy<User, IUserSortingVisitor>? orderBy = null, int? skip = null, int? take = null)
    {
        var query = _context.Users.AsQueryable();
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
            query = visitor.SortItems.Skip(1)
                .Aggregate(orderedQuery, (current, sort) => sort.IsDescending
                    ? current.ThenByDescending(sort.Expr)
                    : current.ThenBy(sort.Expr));
        }

        if (skip.HasValue) query = query.Skip(skip.Value);
        if (take.HasValue) query = query.Take(take.Value);

        var models = await query.ToListAsync();
        foreach (var model in models) await LoadCollectionsAsync(model);
        return models.Select(_aggregateMapper.Map).ToList();
    }

    public Task<int> CountAsync(ISpecification<User, IUserSpecificationVisitor>? specification)
    {
        var query = _context.Users.AsQueryable();
        if (specification == null) return query.CountAsync();
        var visitor = new UserVisitor();
        specification.Accept(visitor);
        if (visitor.Expr != null) query = query.Where(visitor.Expr);

        return query.CountAsync();
    }
    
    private async Task LoadCollectionsAsync(UserModel model)
    {
        await _context.Entry(model).Collection(x => x.Watchlist).LoadAsync();
        await _context.Entry(model).Collection(x => x.History).LoadAsync();
    }
}