using Microsoft.Extensions.Caching.Memory;
using Overoom.Application.Abstractions.Common.Exceptions;
using Overoom.Application.Abstractions.Movie.DTOs;
using Overoom.Application.Abstractions.Movie.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Films.Enums;
using Overoom.Domain.Ratings.Entities;
using Overoom.Domain.Ratings.Specifications;
using Overoom.Domain.Ratings.Specifications.Visitor;
using Overoom.Domain.Specifications;
using Overoom.Domain.Users.Entities;

namespace Overoom.Application.Services.Movie;

public class FilmManager : IFilmManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFilmMapper _mapper;
    private readonly IMemoryCache _memoryCache;

    public FilmManager(IUnitOfWork unitOfWork, IFilmMapper filmMapper, IMemoryCache memoryCache)
    {
        _unitOfWork = unitOfWork;
        _mapper = filmMapper;
        _memoryCache = memoryCache;
    }

    public async Task<FilmDto> GetAsync(Guid id, Guid? userId)
    {
        var film = await GetFilmAsync(id);

        Rating? rating = null;
        User? user = null;
        if (!userId.HasValue) return _mapper.Map(film, rating, user);

        user = await _unitOfWork.UserRepository.Value.GetAsync(userId.Value);
        if (user == null) throw new UserNotFoundException();
        user.AddFilmToHistory(id);
        await _unitOfWork.UserRepository.Value.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();


        var userSpec = new RatingByUserSpecification(userId.Value);
        var filmSpec = new RatingByFilmSpecification(id);
        var ratingList = await
            _unitOfWork.RatingRepository.Value.FindAsync(
                new AndSpecification<Rating, IRatingSpecificationVisitor>(userSpec, filmSpec));
        rating = ratingList.FirstOrDefault();

        return _mapper.Map(film, rating, user);
    }

    public async Task ToggleWatchlistAsync(Guid id, Guid userId)
    {
        await GetFilmAsync(id);
        var user = await _unitOfWork.UserRepository.Value.GetAsync(userId);
        if (user == null) throw new UserNotFoundException();
        if (user.Watchlist.Any(x => x.FilmId == id)) user.RemoveFilmFromWatchlist(id);
        else user.AddFilmToWatchlist(id);
        await _unitOfWork.UserRepository.Value.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<RatingDto> AddRatingAsync(Guid id, Guid userId, double score)
    {
        var user = await _unitOfWork.UserRepository.Value.GetAsync(userId);
        if (user == null) throw new UserNotFoundException();
        var rating = new Rating(id, user.Id, score);
        await _unitOfWork.RatingRepository.Value.AddAsync(rating);
        await _unitOfWork.SaveChangesAsync();

        var film = await GetFilmAsync(id);
        return new RatingDto(film.UserRating, film.UserRatingsCount);
    }

    public async Task<Uri> GetFilmUriAsync(Guid id, CdnType type)
    {
        var film = await GetFilmAsync(id);

        var cdn = film.CdnList.FirstOrDefault(x => x.Type == type);
        if (cdn == null) throw new NotImplementedException();
        return cdn.Uri;
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