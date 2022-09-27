using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Watch2gether.Domain.Abstractions.Repositories;
using Watch2gether.Domain.Films;
using Watch2gether.Domain.Specifications.Abstractions;
using Watch2gether.Domain.Films.Ordering.Visitor;
using Watch2gether.Domain.Films.Specifications.Visitor;
using Watch2gether.Domain.Films.ValueObject;
using Watch2gether.Domain.Ordering.Abstractions;
using Watch2gether.Infrastructure.PersistentStorage.Context;
using Watch2gether.Infrastructure.PersistentStorage.Models.Films;
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

    private Film GetMap(FilmModel model)
    {
        var film = _mapper.Map<Film>(model);
        var x = film.GetType();
        x.GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(film, model.Id);
        return film;
    }

    private FilmModel UpdateMap(Film film, FilmModel model)
    {
        #region Getting old and new values

        var oldGenres = new List<GenreModel>();
        var newGenres = new List<GenreModel>();
        var genres = film.FilmData.Genres;
        foreach (var genre in genres)
        {
            var genreModel = model.Genres.FirstOrDefault(v => v.Name == genre);
            if (genreModel != null)
                oldGenres.Add(genreModel);
            else
                newGenres.Add(new GenreModel {Name = genre});
        }

        var oldScreenWriters = new List<ScreenWriterModel>();
        var newScreenWriters = new List<ScreenWriterModel>();
        var screenWriters = film.FilmData.Screenwriters;
        foreach (var screenWriter in screenWriters)
        {
            var screenWriterModel = model.ScreenWriters.FirstOrDefault(v => v.Name == screenWriter);
            if (screenWriterModel != null)
                oldScreenWriters.Add(screenWriterModel);
            else
                newScreenWriters.Add(new ScreenWriterModel {Name = screenWriter});
        }

        var oldDirectors = new List<DirectorModel>();
        var newDirectors = new List<DirectorModel>();
        var directors = film.FilmData.Directors;
        foreach (var director in directors)
        {
            var directorModel = model.Directors.FirstOrDefault(v => v.Name == director);
            if (directorModel != null)
                oldDirectors.Add(directorModel);
            else
                newDirectors.Add(new DirectorModel {Name = director});
        }

        var oldCountries = new List<CountryModel>();
        var newCountries = new List<CountryModel>();
        var countries = film.FilmData.Countries;
        foreach (var country in countries)
        {
            var countryModel = model.Countries.FirstOrDefault(v => v.Name == country);
            if (countryModel != null)
                oldCountries.Add(countryModel);
            else
                newCountries.Add(new CountryModel {Name = country});
        }

        var oldActors = new List<ActorModel>();
        var newActors = new List<ActorModel>();
        var actors = film.FilmData.Actors;
        foreach (var actor in actors)
        {
            var actorModel =
                model.Actors.FirstOrDefault(v => v.Name == actor.ActorName && v.Description == actor.ActorDescription);
            if (actorModel != null)
                oldActors.Add(actorModel);
            else
                newActors.Add(new ActorModel {Name = actor.ActorName, Description = actor.ActorDescription});
        }

        #endregion

        #region Deliting old values and clear collections

        _context.RemoveRange(model.Genres.Where(x => !oldGenres.Contains(x)));
        _context.RemoveRange(model.Countries.Where(x => !oldCountries.Contains(x)));
        _context.RemoveRange(model.Directors.Where(x => !oldDirectors.Contains(x)));
        _context.RemoveRange(model.ScreenWriters.Where(x => !oldScreenWriters.Contains(x)));
        _context.RemoveRange(model.Actors.Where(x => !oldActors.Contains(x)));
        model.Genres.Clear();
        model.Countries.Clear();
        model.Directors.Clear();
        model.ScreenWriters.Clear();
        model.Actors.Clear();

        #endregion

        _mapper.Map(film, model);

        #region Adding new values and fill collections

        model.Genres.AddRange(oldGenres);
        model.Countries.AddRange(oldCountries);
        model.Directors.AddRange(oldDirectors);
        model.ScreenWriters.AddRange(oldScreenWriters);
        model.Actors.AddRange(oldActors);

        _context.AddRange(newGenres);
        _context.AddRange(newCountries);
        _context.AddRange(newDirectors);
        _context.AddRange(newScreenWriters);
        _context.AddRange(newActors);

        #endregion

        return model;
    }

    public async Task AddAsync(Film entity)
    {
        var film = UpdateMap(entity, new FilmModel());
        await _context.AddAsync(film);
    }

    public async Task AddRangeAsync(IList<Film> entities)
    {
        var films = entities.Select(x => UpdateMap(x, new FilmModel()));
        await _context.AddRangeAsync(films);
    }

    public async Task UpdateAsync(Film entity)
    {
        var model = await _context.Films.Include(x => x.Actors).Include(x => x.Countries).Include(x => x.Directors)
            .Include(x => x.Genres).Include(x => x.ScreenWriters).FirstAsync(x => x.Id == entity.Id);
        UpdateMap(entity, model);
    }

    public async Task UpdateRangeAsync(IList<Film> entities)
    {
        var ids = entities.Select(film => film.Id);
        var films = await _context.Films.Include(x => x.Actors).Include(x => x.Countries).Include(x => x.Directors)
            .Include(x => x.Genres).Include(x => x.ScreenWriters).Where(film => ids.Contains(film.Id)).ToListAsync();
        foreach (var entity in entities)
            UpdateMap(entity, films.First(filmModel => filmModel.Id == entity.Id));
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
            .ForMember(x => x.Description, expression => expression.MapFrom(y => y.FilmData.Description))
            .ForMember(x => x.Rating, expression => expression.MapFrom(y => y.FilmData.Rating))
            .ForMember(x => x.Year, expression => expression.MapFrom(y => y.FilmData.Year))
            .ForMember(x => x.CountEpisodes, expression => expression.MapFrom(y => y.FilmData.CountEpisodes))
            .ForMember(x => x.CountSeasons, expression => expression.MapFrom(y => y.FilmData.CountSeasons))
            .ForMember(x => x.ShortDescription, expression => expression.MapFrom(y => y.FilmData.ShortDescription))
            .ForMember(x => x.Name, expression => expression.MapFrom(y => y.FilmData.Name));


        expr.CreateMap<FilmModel, Film>().ForMember(x => x.Url, expression => expression.MapFrom(y => new Uri(y.Url)))
            .ForMember(x => x.FilmData.Genres, expression => expression.MapFrom(y => y.Genres.Select(x => x.Name)))
            .ForMember(x => x.FilmData.Screenwriters,
                expression => expression.MapFrom(y => y.ScreenWriters.Select(x => x.Name)))
            .ForMember(x => x.FilmData.Directors,
                expression => expression.MapFrom(y => y.Directors.Select(x => x.Name)))
            .ForMember(x => x.FilmData.Countries,
                expression => expression.MapFrom(y => y.Countries.Select(x => x.Name)));


        expr.CreateMap<ActorModel, ActorData>()
            .ForMember(x => x.ActorName, expression => expression.MapFrom(y => y.Name))
            .ForMember(x => x.ActorDescription, expression => expression.MapFrom(y => y.Description));
    }));
}