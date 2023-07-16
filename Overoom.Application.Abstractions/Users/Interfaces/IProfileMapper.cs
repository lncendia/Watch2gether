using Overoom.Application.Abstractions.Users.DTOs;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Ratings;
using Overoom.Domain.Users.Entities;

namespace Overoom.Application.Abstractions.Users.Interfaces;

public interface IProfileMapper
{
    ProfileDto Map(User user, IEnumerable<Film> history, IEnumerable<Film> watchlist);
    RatingDto Map(Rating rating, Film film);
}