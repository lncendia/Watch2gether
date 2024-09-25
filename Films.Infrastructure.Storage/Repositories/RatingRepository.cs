using Microsoft.EntityFrameworkCore;
using Films.Domain.Abstractions.Repositories;
using Films.Domain.Ordering.Abstractions;
using Films.Domain.Ratings;
using Films.Domain.Ratings.Ordering.Visitor;
using Films.Domain.Ratings.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Ratings;
using Films.Infrastructure.Storage.Visitors.Sorting;
using Films.Infrastructure.Storage.Visitors.Specifications;

namespace Films.Infrastructure.Storage.Repositories;

public class RatingRepository(
    ApplicationDbContext context,
    IAggregateMapperUnit<Rating, RatingModel> aggregateMapper,
    IModelMapperUnit<RatingModel, Rating> modelMapper)
    : IRatingRepository
{
    public async Task AddAsync(Rating entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        var rating = await modelMapper.MapAsync(entity);
        await context.AddAsync(rating);
    }

    public async Task UpdateAsync(Rating entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        await modelMapper.MapAsync(entity);
    }
    

    public Task DeleteAsync(Guid id)
    {
        context.Remove(context.Ratings.First(rating => rating.Id ==id));
        return Task.CompletedTask;
    }

    public async Task<Rating?> GetAsync(Guid id)
    {
        var rating = await context.Ratings.AsNoTracking().FirstOrDefaultAsync(ratingModel => ratingModel.Id == id);
        return await (rating == null ? null : aggregateMapper.MapAsync(rating))!;
    }

    public async Task<IReadOnlyCollection<Rating>> FindAsync(
        ISpecification<Rating, IRatingSpecificationVisitor>? specification,
        IOrderBy<Rating, IRatingSortingVisitor>? orderBy = null, int? skip = null, int? take = null)
    {
        var query = context.Ratings.AsQueryable();
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
            
        var entities = await query.AsNoTracking().ToArrayAsync();
        var aggregates = new Rating[entities.Length];
        for (var i = 0; i < aggregates.Length; i++) aggregates[i] = await aggregateMapper.MapAsync(entities[i]);
        return aggregates;
    }

    public Task<int> CountAsync(ISpecification<Rating, IRatingSpecificationVisitor>? specification)
    {
        var query = context.Ratings.AsQueryable();
        if (specification == null) return query.CountAsync();
        var visitor = new RatingVisitor();
        specification.Accept(visitor);
        if (visitor.Expr != null) query = query.Where(visitor.Expr);

        return query.CountAsync();
    }
}