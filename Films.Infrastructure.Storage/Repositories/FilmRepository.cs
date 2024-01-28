using Microsoft.EntityFrameworkCore;
using Films.Domain.Abstractions.Repositories;
using Films.Domain.Films.Entities;
using Films.Domain.Films.Ordering.Visitor;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using Films.Domain.Ordering.Abstractions;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Film;
using Films.Infrastructure.Storage.Visitors.Sorting;
using Films.Infrastructure.Storage.Visitors.Specifications;

namespace Films.Infrastructure.Storage.Repositories;

public class FilmRepository : IFilmRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IAggregateMapperUnit<Film, FilmModel> _aggregateMapper;
    private readonly IModelMapperUnit<FilmModel, Film> _modelMapper;

    public FilmRepository(ApplicationDbContext context, IAggregateMapperUnit<Film, FilmModel> aggregateMapper,
        IModelMapperUnit<FilmModel, Film> modelMapper)
    {
        _context = context;
        _aggregateMapper = aggregateMapper;
        _modelMapper = modelMapper;
    }

    public async Task AddAsync(Film entity)
    {
        _context.Notifications.AddRange(entity.DomainEvents);
        var film = await _modelMapper.MapAsync(entity);
        await _context.AddAsync(film);
    }

    public async Task UpdateAsync(Film entity)
    {
        _context.Notifications.AddRange(entity.DomainEvents);
        await _modelMapper.MapAsync(entity);
    }

    public Task DeleteAsync(Guid id)
    {
        _context.Remove(_context.Films.First(film => film.Id == id));
        return Task.CompletedTask;
    }

    public async Task<Film?> GetAsync(Guid id)
    {
        var film = await _context.Films.FirstOrDefaultAsync(userModel => userModel.Id == id);
        if (film == null) return null;
        await LoadCollectionsAsync(film);
        return _aggregateMapper.Map(film);
    }

    public async Task<IList<Film>> FindAsync(ISpecification<Film, IFilmSpecificationVisitor>? specification,
        IOrderBy<Film, IFilmSortingVisitor>? orderBy = null, int? skip = null, int? take = null)
    {
        var query = _context.Films.AsQueryable();
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

    public Task<int> CountAsync(ISpecification<Film, IFilmSpecificationVisitor>? specification)
    {
        var query = _context.Films.AsQueryable();
        if (specification == null) return query.CountAsync();
        var visitor = new FilmVisitor();
        specification.Accept(visitor);
        if (visitor.Expr != null) query = query.Where(visitor.Expr);

        return query.CountAsync();
    }

    private async Task LoadCollectionsAsync(FilmModel model)
    {
        await _context.Entry(model).Collection(x => x.Actors).Query().Include(x => x.Person).LoadAsync();
        await _context.Entry(model).Collection(x => x.Countries).LoadAsync();
        await _context.Entry(model).Collection(x => x.Directors).LoadAsync();
        await _context.Entry(model).Collection(x => x.Genres).LoadAsync();
        await _context.Entry(model).Collection(x => x.ScreenWriters).LoadAsync();
        await _context.Entry(model).Collection(x => x.CdnList).Query().Include(x => x.Voices).LoadAsync();
    }
}