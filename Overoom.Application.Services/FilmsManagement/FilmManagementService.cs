using Microsoft.Extensions.Caching.Memory;
using Overoom.Application.Abstractions.FilmsManagement.DTOs;
using Overoom.Application.Abstractions.FilmsManagement.Exceptions;
using Overoom.Application.Abstractions.FilmsManagement.Interfaces;
using Overoom.Application.Abstractions.Movie.Exceptions;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Films.Ordering;
using Overoom.Domain.Films.Ordering.Visitor;
using Overoom.Domain.Films.Specifications;
using Overoom.Domain.Films.Specifications.Visitor;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Specifications.Abstractions;
using CdnDto = Overoom.Domain.Films.DTOs.CdnDto;
using FilmShortDto = Overoom.Application.Abstractions.FilmsManagement.DTOs.FilmShortDto;

namespace Overoom.Application.Services.FilmsManagement;

public class FilmManagementService : IFilmManagementService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFilmPosterService _filmPosterService;
    private readonly IMemoryCache _memoryCache;
    private readonly IFilmManagementMapper _mapper;

    public FilmManagementService(IUnitOfWork unitOfWork, IFilmPosterService filmPosterService, IMemoryCache memoryCache,
        IFilmManagementMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _filmPosterService = filmPosterService;
        _memoryCache = memoryCache;
        _mapper = mapper;
    }

    public async Task LoadAsync(LoadDto film)
    {
        Uri? poster;
        if (film.PosterUri != null) poster = await _filmPosterService.SaveAsync(film.PosterUri);
        else if (film.PosterStream != null) poster = await _filmPosterService.SaveAsync(film.PosterStream);
        else throw new PosterMissingException();
        var builder = FilmBuilder.Create()
            .WithName(film.Name)
            .WithDescription(film.Description)
            .WithYear(film.Year)
            .WithRating(film.Rating)
            .WithType(film.Type)
            .WithCdn(film.CdnList.Select(x => new CdnDto(x.Type, x.Uri, x.Quality, x.Voices)))
            .WithGenres(film.Genres)
            .WithActors(film.Actors)
            .WithDirectors(film.Directors)
            .WithScreenwriters(film.Screenwriters)
            .WithCountries(film.Countries)
            .WithPoster(poster);
        if (!string.IsNullOrEmpty(film.ShortDescription)) builder = builder.WithShortDescription(film.ShortDescription);
        if (film is { CountSeasons: not null, CountEpisodes: not null })
            builder = builder.WithEpisodes(film.CountSeasons.Value, film.CountEpisodes.Value);
        await _unitOfWork.FilmRepository.Value.AddAsync(builder.Build());
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ChangeAsync(ChangeDto toChange)
    {
        var film = await GetFilmAsync(toChange.FilmId);
        if (!string.IsNullOrEmpty(toChange.Description)) film.Description = toChange.Description;
        if (!string.IsNullOrEmpty(toChange.ShortDescription)) film.ShortDescription = toChange.ShortDescription;
        if (toChange.Rating.HasValue) film.Rating = toChange.Rating.Value;
        if (toChange is { CountSeasons: not null, CountEpisodes: not null })
            film.UpdateSeriesInfo(toChange.CountSeasons.Value, toChange.CountEpisodes.Value);

        Uri? poster = null;
        if (toChange.PosterUri != null) poster = await _filmPosterService.SaveAsync(toChange.PosterUri);
        else if (toChange.PosterStream != null)
            poster = await _filmPosterService.SaveAsync(toChange.PosterStream);

        if (poster != null)
        {
            await _filmPosterService.DeleteAsync(film.PosterUri);
            film.PosterUri = poster;
        }

        if (toChange.CdnList != null)
        {
            foreach (var cdnDto in toChange.CdnList)
            {
                film.AddOrChangeCdn(new CdnDto(cdnDto.Type, cdnDto.Uri, cdnDto.Quality, cdnDto.Voices));
            }
        }

        await _unitOfWork.FilmRepository.Value.UpdateAsync(film);
        await _unitOfWork.SaveChangesAsync();
        _memoryCache.Remove(film.Id);
    }

    public async Task DeleteAsync(Guid filmId)
    {
        var film = await GetFilmAsync(filmId);
        await _filmPosterService.DeleteAsync(film.PosterUri);
        await _unitOfWork.FilmRepository.Value.DeleteAsync(filmId);
        await _unitOfWork.SaveChangesAsync();
        _memoryCache.Remove(filmId);
    }

    public async Task<GetDto> GetAsync(Guid filmId)
    {
        var film = await GetFilmAsync(filmId);
        return _mapper.MapGet(film);
    }

    public async Task<List<FilmShortDto>> FindAsync(int page, string? query = null)
    {
        ISpecification<Film, IFilmSpecificationVisitor>? specification = null;

        if (!string.IsNullOrEmpty(query)) specification = new FilmByNameSpecification(query);

        IOrderBy<Film, IFilmSortingVisitor> orderBy = new FilmOrderByDate();

        var films = await _unitOfWork.FilmRepository.Value.FindAsync(specification, orderBy, (page - 1) * 10, 10);
        return films.Select(_mapper.MapShort).ToList();
    }


    private async Task<Film> GetFilmAsync(Guid id)
    {
        if (!_memoryCache.TryGetValue(id, out Film? film))
        {
            film = await _unitOfWork.FilmRepository.Value.GetAsync(id);
            if (film == null) throw new FilmNotFoundException();
            _memoryCache.Set(id, film, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
        }
        else
        {
            if (film == null) throw new FilmNotFoundException();
        }

        return film;
    }
}