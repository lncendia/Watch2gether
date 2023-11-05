using Microsoft.EntityFrameworkCore;
using Overoom.Domain.Abstractions.Repositories;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Ratings.Entities;
using Overoom.Domain.Ratings.Ordering.Visitor;
using Overoom.Domain.Ratings.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Infrastructure.Storage.Context;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Models.Rating;
using Overoom.Infrastructure.Storage.Visitors.Sorting;
using Overoom.Infrastructure.Storage.Visitors.Specifications;

namespace Overoom.Infrastructure.Storage.Repositories;

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

    public async Task<IList<Rating>> FindAsync(ISpecification<Rating, IRatingSpecificationVisitor>? specification,
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

        return (await query.ToListAsync()).Select(_aggregateMapper.Map).ToList();
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