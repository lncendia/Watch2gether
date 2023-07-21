using Overoom.Application.Abstractions.Profile.DTOs;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Ratings;
using Overoom.Domain.Ratings.Entities;
using Overoom.Domain.Users.Entities;

namespace Overoom.Application.Abstractions.Profile.Interfaces;

public interface IProfileMapper
{
    ProfileDto Map(User user, IEnumerable<Film> history, IEnumerable<Film> watchlist);
    RatingDto Map(Rating rating, Film film);
}