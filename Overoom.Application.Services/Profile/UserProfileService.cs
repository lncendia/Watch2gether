using Overoom.Application.Abstractions.Common.Exceptions;
using Overoom.Application.Abstractions.Profile.DTOs;
using Overoom.Application.Abstractions.Profile.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Films.Specifications;
using Overoom.Domain.Ordering;
using Overoom.Domain.Ratings.Entities;
using Overoom.Domain.Ratings.Ordering;
using Overoom.Domain.Ratings.Ordering.Visitor;
using Overoom.Domain.Ratings.Specifications;
using IUserProfileService = Overoom.Application.Abstractions.Profile.Interfaces.IUserProfileService;

namespace Overoom.Application.Services.Profile;

public class UserProfileService : IUserProfileService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProfileMapper _profileMapper;

    public UserProfileService(IUnitOfWork unitOfWork, IProfileMapper profileMapper)
    {
        _unitOfWork = unitOfWork;
        _profileMapper = profileMapper;
    }

    public async Task<ProfileDto> GetProfileAsync(Guid id)
    {
        var user = await _unitOfWork.UserRepository.Value.GetAsync(id);
        if (user == null) throw new UserNotFoundException();

        var history = user.History.OrderByDescending(x => x.Date).Select(x => x.FilmId).ToList();
        var watchlist = user.Watchlist.OrderByDescending(x => x.Date).Select(x => x.FilmId).ToList();

        var watchlistSpec = new FilmByIdsSpecification(watchlist);
        var historySpec = new FilmByIdsSpecification(history);

        var watchlistFilms = await _unitOfWork.FilmRepository.Value.FindAsync(watchlistSpec);
        var historyFilms = await _unitOfWork.FilmRepository.Value.FindAsync(historySpec);
        return _profileMapper.Map(user, historyFilms.OrderBy(film => history.IndexOf(film.Id)),
            watchlistFilms.OrderBy(film => watchlist.IndexOf(film.Id)));
    }

    public async Task<IReadOnlyCollection<RatingDto>> GetRatingsAsync(Guid id, int page)
    {
        var spec = new RatingByUserSpecification(id);
        var ratings =
            await _unitOfWork.RatingRepository.Value.FindAsync(spec,
                new DescendingOrder<Rating, IRatingSortingVisitor>(new RatingOrderByDate()), (page - 1) * 10, 10);
        var films = await _unitOfWork.FilmRepository.Value.FindAsync(
            new FilmByIdsSpecification(ratings.Select(x => x.FilmId)));
        return ratings.Select(x => _profileMapper.Map(x, films.First(f => f.Id == x.FilmId))).ToList();
    }

    public async Task<IReadOnlyCollection<string>> GetGenresAsync(Guid id)
    {
        var user = await _unitOfWork.UserRepository.Value.GetAsync(id);
        if (user == null) throw new UserNotFoundException();
        return user.Genres;
    }
}