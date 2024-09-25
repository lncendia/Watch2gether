using Microsoft.EntityFrameworkCore;
using Films.Domain.Abstractions.Repositories;
using Films.Domain.Films;
using Films.Domain.Films.Ordering.Visitor;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using Films.Domain.Ordering.Abstractions;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Extensions;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Films;
using Films.Infrastructure.Storage.Visitors.Sorting;
using Films.Infrastructure.Storage.Visitors.Specifications;

namespace Films.Infrastructure.Storage.Repositories;

public class FilmRepository(
    ApplicationDbContext context,
    IAggregateMapperUnit<Film, FilmModel> aggregateMapper,
    IModelMapperUnit<FilmModel, Film> modelMapper)
    : IFilmRepository
{
    public async Task AddAsync(Film entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        var film = await modelMapper.MapAsync(entity);
        await context.AddAsync(film);
    }

    public async Task UpdateAsync(Film entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        await modelMapper.MapAsync(entity);
    }

    public Task DeleteAsync(Guid id)
    {
        context.Remove(context.Films.First(film => film.Id == id));
        return Task.CompletedTask;
    }

    public async Task<Film?> GetAsync(Guid id)
    {
        var film = await context.Films
            .LoadDependencies()
            .AsNoTracking()
            .FirstOrDefaultAsync(userModel => userModel.Id == id);

        return await (film == null ? null : aggregateMapper.MapAsync(film))!;
    }

    public async Task<IReadOnlyCollection<Film>> FindAsync(
        ISpecification<Film, IFilmSpecificationVisitor>? specification,
        IOrderBy<Film, IFilmSortingVisitor>? orderBy = null, int? skip = null, int? take = null)
    {
        var query = context.Films.AsQueryable();
        if (specification != null)
        {
            var visitor = new FilmVisitor();
            specification.Accept(visitor);
            if (visitor.Expr != null) query = query.Where(visitor.Expr);
        }

        if (orderBy != null)
        {
            var visitor = new FilmSortingVisitor();
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
        var aggregates = new Film[entities.Length];
        for (var i = 0; i < aggregates.Length; i++) aggregates[i] = await aggregateMapper.MapAsync(entities[i]);
        return aggregates;
    }

    public Task<int> CountAsync(ISpecification<Film, IFilmSpecificationVisitor>? specification)
    {
        var query = context.Films.AsQueryable();
        if (specification == null) return query.CountAsync();
        var visitor = new FilmVisitor();
        specification.Accept(visitor);
        if (visitor.Expr != null) query = query.Where(visitor.Expr);

        return query.CountAsync();
    }
}