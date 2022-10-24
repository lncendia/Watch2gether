using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Overoom.Domain.Abstractions.Repositories;
using Overoom.Infrastructure.PersistentStorage.Context;
using Overoom.Infrastructure.PersistentStorage.Models.Users;
using Overoom.Infrastructure.PersistentStorage.Visitors.Sorting;
using Overoom.Infrastructure.PersistentStorage.Visitors.Specifications;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.Users;
using Overoom.Domain.Users.Ordering.Visitor;
using Overoom.Domain.Users.Specifications.Visitor;
using Overoom.Infrastructure.PersistentStorage.Models;

namespace Overoom.Infrastructure.PersistentStorage.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
        _mapper = GetMapper();
    }

    private User Map(UserModel model)
    {
        var user = _mapper.Map<User>(model);
        var x = user.GetType();
        x.GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(user, model.Id);
        return user;
    }

    public async Task AddAsync(User entity)
    {
        var user = _mapper.Map<User, UserModel>(entity);
        await _context.AddAsync(user);
    }

    public async Task AddRangeAsync(IList<User> entities)
    {
        var users = _mapper.Map<IList<User>, List<UserModel>>(entities);
        await _context.AddRangeAsync(users);
    }

    public async Task UpdateAsync(User entity)
    {
        var model = await _context.Users.FirstAsync(x => x.Id == entity.Id);
        _mapper.Map(entity, model);
    }

    public async Task UpdateRangeAsync(IList<User> entities)
    {
        var ids = entities.Select(user => user.Id);
        var users = await _context.Users.Where(user => ids.Contains(user.Id)).ToListAsync();
        foreach (var entity in entities)
            _mapper.Map(entity, users.First(userModel => userModel.Id == entity.Id));
    }

    public Task DeleteAsync(User entity)
    {
        _context.Remove(_context.Users.First(user => user.Id == entity.Id));
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<User> entities)
    {
        var ids = entities.Select(user => user.Id);
        _context.RemoveRange(_context.Users.Where(user => ids.Contains(user.Id)));
        return Task.CompletedTask;
    }

    public async Task<User?> GetAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(userModel => userModel.Id == id);
        return user == null ? null : Map(user);
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

        return (await query.ToListAsync()).Select(Map).ToList();
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

    private static IMapper GetMapper() => new Mapper(new MapperConfiguration(expr =>
    {
        expr.CreateMap<User, UserModel>().ReverseMap();
    }));
}