using Microsoft.EntityFrameworkCore;
using Films.Domain.Abstractions.Repositories;
using Films.Domain.Ordering.Abstractions;
using Films.Domain.Ratings.Entities;
using Films.Domain.Ratings.Ordering.Visitor;
using Films.Domain.Ratings.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Rating;
using Films.Infrastructure.Storage.Visitors.Sorting;
using Films.Infrastructure.Storage.Visitors.Specifications;

namespace Films.Infrastructure.Storage.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IAggregateMapperUnit<Rating, RatingModel> _aggregateMapper;
    private readonly IModelMapperUnit<RatingModel, Rating> _modelMapper;

    public RatingRepository(ApplicationDbContext context, IAggregateMapperUnit<Rating, RatingModel> aggregateMapper,
        IModelMapperUnit<RatingModel, Rating> modelMapper)
    {
        _context = context;
        _aggregateMapper = aggregateMapper;
        _modelMapper = modelMapper;
    }

    public async Task AddAsync(Rating entity)
    {
        _context.Notifications.AddRange(entity.DomainEvents);
        var rating = await _modelMapper.MapAsync(entity);
        await _context.AddAsync(rating);
    }

    public async Task UpdateAsync(Rating entity)
    {
        _context.Notifications.AddRange(entity.DomainEvents);
        await _modelMapper.MapAsync(entity);
    }
    

    public Task DeleteAsync(Guid id)
    {
        _context.Remove(_context.Ratings.First(rating => rating.Id ==id));
        return Task.CompletedTask;
    }

    public async Task<Rating?> GetAsync(Guid id)
    {
        var rating = await _context.Ratings.FirstOrDefaultAsync(ratingModel => ratingModel.Id == id);
        return rating == null ? null : _aggregateMapper.Map(rating);
    }

    public async Task<IReadOnlyCollection<Rating>> FindAsync(
        ISpecification<Rating, IRatingSpecificationVisitor>? specification,
        IOrderBy<Rating, IRatingSortingVisitor>? orderBy = null, int? skip = null, int? take = null)
    {
        var query = _context.Ratings.AsQueryable();
        if (specification != null)
        {
            var visitor = new RatingVisitor();
            specification.Accept(visitor);
            if (visitor.Expr != null) query = query.Where(visitor.Expr);
        }

        if (orderBy != null)
        {
            var visitor = new RatingSortingVisitor();
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

        return (await query.ToArrayAsync()).Select(_aggregateMapper.Map).ToArray();
    }

    public Task<int> CountAsync(ISpecification<Rating, IRatingSpecificationVisitor>? specification)
    {
        var query = _context.Ratings.AsQueryable();
        if (specification == null) return query.CountAsync();
        var visitor = new RatingVisitor();
        specification.Accept(visitor);
        if (visitor.Expr != null) query = query.Where(visitor.Expr);

        return query.CountAsync();
    }
}