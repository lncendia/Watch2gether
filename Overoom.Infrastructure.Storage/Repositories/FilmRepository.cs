using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Overoom.Domain.Abstractions.Repositories;
using Overoom.Domain.Film.Entities;
using Overoom.Domain.Film.Ordering.Visitor;
using Overoom.Domain.Film.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Infrastructure.Storage.Context;
using Overoom.Infrastructure.Storage.Models.Films;
using Overoom.Infrastructure.Storage.Visitors.Sorting;
using Overoom.Infrastructure.Storage.Visitors.Specifications;

namespace Overoom.Infrastructure.Storage.Repositories;

public class FilmRepository : IFilmRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public FilmRepository(ApplicationDbContext context)
    {
        _context = context;
        _mapper = GetMapper();
    }

    private Film GetMap(FilmModel model)
    {
        var film = _mapper.Map<Film>(model);
        var x = film.GetType();
        x.GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(film, model.Id);
        return film;
    }

    private FilmModel AddMap(Film film)
    {
        var model = new FilmModel
        {
            Genres = film.FilmTags.Genres.Select(x => new GenreModel {Name = x}).ToList(),
            Countries = film.FilmTags.Countries.Select(x => new CountryModel {Name = x}).ToList(),
            Directors = film.FilmTags.Directors.Select(x => new DirectorModel {Name = x}).ToList(),
            ScreenWriters = film.FilmTags.Screenwriters.Select(x => new ScreenWriterModel {Name = x}).ToList(),
            Actors = film.FilmTags.Actors
                .Select(x => new ActorModel {Name = x.ActorName, Description = x.ActorDescription}).ToList()
        };
        _mapper.Map(film, model);
        return model;
    }

    public async Task AddAsync(Film entity)
    {
        var film = AddMap(entity);
        await _context.AddAsync(film);
    }

    public async Task AddRangeAsync(IList<Film> entities)
    {
        var films = entities.Select(AddMap);
        await _context.AddRangeAsync(films);
    }

    public async Task UpdateAsync(Film entity)
    {
        var model = await _context.Films.FirstAsync(x => x.Id == entity.Id);
        _mapper.Map(entity, model);
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

    public Task DeleteRangeAsync(IEnumerable<Film> entities)
    {
        var ids = entities.Select(film => film.Id);
        _context.RemoveRange(_context.Films.Where(film => ids.Contains(film.Id)));
        return Task.CompletedTask;
    }

    public async Task<Film?> GetAsync(Guid id)
    {
        var film = await _context.Films.Include(x => x.Actors).Include(x => x.Countries).Include(x => x.Directors)
            .Include(x => x.Genres).Include(x => x.ScreenWriters).FirstOrDefaultAsync(userModel => userModel.Id == id);
        return film == null ? null : GetMap(film);
    }

    public async Task<IList<Film>> FindAsync(ISpecification<Film, IFilmSpecificationVisitor>? specification,
        IOrderBy<Film, IFilmSortingVisitor>? orderBy = null, int? skip = null, int? take = null)
    {
        var query = _context.Films.Include(x => x.Actors).Include(x => x.Countries).Include(x => x.Directors)
            .Include(x => x.Genres).Include(x => x.ScreenWriters).AsQueryable();
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

        return (await query.ToListAsync()).Select(GetMap).ToList();
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
            .ForMember(x => x.Description, expression => expression.MapFrom(y => y.FilmInfo.Description))
            .ForMember(x => x.Rating, expression => expression.MapFrom(y => y.FilmInfo.Rating))
            .ForMember(x => x.CountEpisodes, expression => expression.MapFrom(y => y.FilmInfo.CountEpisodes))
            .ForMember(x => x.CountSeasons, expression => expression.MapFrom(y => y.FilmInfo.CountSeasons))
            .ForMember(x => x.ShortDescription, expression => expression.MapFrom(y => y.FilmInfo.ShortDescription));

        expr.CreateMap<FilmModel, Film>()
            .ForMember(x => x.Url, expression => expression.MapFrom(y => new Uri(y.Url)))
            .ForMember(x => x.FilmInfo, expression => expression.Ignore())
            .ForMember(x => x.FilmCollections, expression => expression.Ignore());

        expr.CreateMap<ActorModel, ValueTuple<string, string>>()
            .ForMember(x => x.Item1, expression => expression.MapFrom(y => y.Name))
            .ForMember(x => x.Item2, expression => expression.MapFrom(y => y.Description));
        
        expr.CreateMap<GenreModel, string>().ConvertUsing(x => x.Name);
        expr.CreateMap<ScreenWriterModel, string>().ConvertUsing(x => x.Name);
        expr.CreateMap<DirectorModel, string>().ConvertUsing(x => x.Name);
        expr.CreateMap<CountryModel, string>().ConvertUsing(x => x.Name);
    }));
}