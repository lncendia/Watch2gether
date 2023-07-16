using Overoom.Application.Abstractions.Movie.DTOs;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Ratings;
using Overoom.Domain.Users.Entities;

namespace Overoom.Application.Abstractions.Movie.Interfaces;

public interface IFilmMapper
{
    public FilmDto Map(Film film, Rating? rating, User? user);
}