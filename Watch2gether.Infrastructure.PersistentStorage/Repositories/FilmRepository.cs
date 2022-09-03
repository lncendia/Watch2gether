using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Watch2gether.Domain.Abstractions.Repositories;
using Watch2gether.Domain.Films;
using Watch2gether.Domain.Specifications.Abstractions;
using Watch2gether.Domain.Films.Ordering.Visitor;
using Watch2gether.Domain.Films.Specifications.Visitor;
using Watch2gether.Domain.Ordering.Abstractions;
using Watch2gether.Infrastructure.PersistentStorage.Context;
using Watch2gether.Infrastructure.PersistentStorage.Models;
using Watch2gether.Infrastructure.PersistentStorage.Visitors.Sorting;
using Watch2gether.Infrastructure.PersistentStorage.Visitors.Specifications;

namespace Watch2gether.Infrastructure.PersistentStorage.Repositories;

public class FilmRepository : IFilmRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public FilmRepository(ApplicationDbContext context)
    {
        _context = context;
        _mapper = GetMapper();
    }

    private Film Map(FilmModel model)
    {
        var film = _mapper.Map<Film>(model);
        var x = film.GetType();
        x.GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(film, model.Id);
        return film;
    }

    public async Task AddAsync(Film entity)
    {
        var film = _mapper.Map<Film, FilmModel>(entity);
        await _context.AddAsync(film);
    }

    public async Task AddRangeAsync(IList<Film> entities)
    {
        var films = _mapper.Map<IList<Film>, List<FilmModel>>(entities);
        await _context.AddRangeAsync(films);
    }

    public Task UpdateAsync(Film entity)
    {
        var model = _context.Films.First(x => x.Id == entity.Id);
        _mapper.Map(entity, model);
        return Task.CompletedTask;
    }

    public async Task UpdateRangeAsync(IList<Film> entities)
    {
        var ids = entities.Select(film => film.Id);
        var films = await _context.Films.Where(film => ids.Contains(film.Id)).ToListAsync();
        foreach (var entity in entities)
            _mapper.Map(entity, films.First(filmModel => filmModel.Id == entity.Id));
    }

    public Task DeleteAsync(Film entity)
    {
        _context.Remove(_context.Films.First(film => film.Id == entity.Id));
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IList<Film> entities)
    {
        var ids = entities.Select(film => film.Id);
        _context.RemoveRange(_context.Films.Where(film => ids.Contains(film.Id)));
        return Task.CompletedTask;
    }

    public async Task<Film?> GetAsync(Guid id)
    {
        var film = await _context.Films.FirstOrDefaultAsync(userModel => userModel.Id == id);
        return film == null ? null : Map(film);
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

        return (await query.ToListAsync()).Select(Map).ToList();
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

    private static IMapper GetMapper() => new Mapper(new MapperConfiguration(expr =>
    {
        expr.CreateMap<Film, FilmModel>()
            .ForMember(x => x.Url, expression => expression.MapFrom(y => y.Url.AbsoluteUri))
            .ForMember(x => x.Actors,
                expression => expression.MapFrom(y =>
                    y.FilmData.Actors.Select(x => new ValueTuple<string, string>(x.ActorName, x.ActorDescription))))
            .ForMember(x => x.Countries, expression => expression.MapFrom(y => y.FilmData.Countries))
            .ForMember(x => x.Description, expression => expression.MapFrom(y => y.FilmData.Description))
            .ForMember(x => x.Directors, expression => expression.MapFrom(y => y.FilmData.Directors))
            .ForMember(x => x.Genres, expression => expression.MapFrom(y => y.FilmData.Genres))
            .ForMember(x => x.Rating, expression => expression.MapFrom(y => y.FilmData.Rating))
            .ForMember(x => x.ScreenWriters, expression => expression.MapFrom(y => y.FilmData.Screenwriters))
            .ForMember(x => x.Year, expression => expression.MapFrom(y => y.FilmData.Year))
            .ForMember(x => x.CountEpisodes, expression => expression.MapFrom(y => y.FilmData.CountEpisodes))
            .ForMember(x => x.CountSeasons, expression => expression.MapFrom(y => y.FilmData.CountSeasons))
            .ForMember(x => x.ShortDescription, expression => expression.MapFrom(y => y.FilmData.ShortDescription))
            .ForMember(x => x.Name, expression => expression.MapFrom(y => y.FilmData.Name));
        expr.CreateMap<FilmModel, Film>().ForMember(x => x.Url, expression => expression.MapFrom(y => new Uri(y.Url)));
    }));
}