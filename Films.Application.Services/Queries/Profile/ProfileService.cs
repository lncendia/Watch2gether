using Films.Application.Abstractions.Common.Exceptions;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Domain.Films.Specifications;
using Films.Domain.Ordering;
using Films.Domain.Ratings.Entities;
using Films.Domain.Ratings.Ordering;
using Films.Domain.Ratings.Ordering.Visitor;
using Films.Domain.Ratings.Specifications;

namespace Films.Application.Services.Profile;

public class ProfileService(IUnitOfWork unitOfWork, IProfileMapper profileMapper) : IProfileService
{
    public async Task<ProfileDto> GetProfileAsync(Guid id)
    {
        var user = await unitOfWork.UserRepository.Value.GetAsync(id);
        if (user == null) throw new UserNotFoundException();

        var history = user.History.OrderByDescending(x => x.Date).Select(x => x.FilmId).ToList();
        var watchlist = user.Watchlist.OrderByDescending(x => x.Date).Select(x => x.FilmId).ToList();

        var watchlistSpec = new FilmByIdsSpecification(watchlist);
        var historySpec = new FilmByIdsSpecification(history);

        var watchlistFilms = await unitOfWork.FilmRepository.Value.FindAsync(watchlistSpec);
        var historyFilms = await unitOfWork.FilmRepository.Value.FindAsync(historySpec);
        return profileMapper.Map(user, historyFilms.OrderBy(film => history.IndexOf(film.Id)),
            watchlistFilms.OrderBy(film => watchlist.IndexOf(film.Id)));
    }

    public async Task<IReadOnlyCollection<RatingDto>> GetRatingsAsync(Guid id, int page)
    {
        var spec = new RatingByUserSpecification(id);
        var ratings =
            await unitOfWork.RatingRepository.Value.FindAsync(spec,
                new DescendingOrder<Rating, IRatingSortingVisitor>(new RatingOrderByDate()), (page - 1) * 10, 10);
        var films = await unitOfWork.FilmRepository.Value.FindAsync(
            new FilmByIdsSpecification(ratings.Select(x => x.FilmId)));
        return ratings.Select(x => profileMapper.Map(x, films.First(f => f.Id == x.FilmId))).ToList();
    }

    public async Task<IReadOnlyCollection<string>> GetGenresAsync(Guid id)
    {
        var user = await unitOfWork.UserRepository.Value.GetAsync(id);
        if (user == null) throw new UserNotFoundException();
        return user.Genres;
    }
}