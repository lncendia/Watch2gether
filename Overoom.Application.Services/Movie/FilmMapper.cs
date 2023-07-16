using Overoom.Application.Abstractions.Movie.DTOs;
using Overoom.Application.Abstractions.Movie.Interfaces;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Films.Enums;
using Overoom.Domain.Ratings;
using Overoom.Domain.Users.Entities;

namespace Overoom.Application.Services.Movie;

public class FilmMapper : IFilmMapper
{
    public FilmDto Map(Film film, Rating? rating, User? user)
    {
        var x = FilmDtoBuilder.Create()
            .WithId(film.Id)
            .WithName(film.Name)
            .WithYear(film.Year)
            .WithType(film.Type)
            .WithPoster(film.PosterUri)
            .WithDescription(film.Description)
            .WithRating(film.Rating)
            .WithUserRating(film.UserRating, film.UserRatingsCount)
            .WithDirectors(film.FilmTags.Directors)
            .WithScreenwriters(film.FilmTags.Screenwriters)
            .WithGenres(film.FilmTags.Genres)
            .WithCountries(film.FilmTags.Countries)
            .WithActors(film.FilmTags.Actors.Select(x => (x.ActorName, x.ActorDescription)).ToList())
            .WithCdn(film.CdnList.Select(x => new CdnDto(x.Type, x.Quality, x.Voices)).ToList());

        if (rating != null) x = x.WithUserScore(rating.Score);
        if (user != null) x = x.WithWatchlist(user.Watchlist.Any(note => note.FilmId == film.Id));
        if (film.Type == FilmType.Serial)
            x = x.WithEpisodes(film.CountSeasons!.Value, film.CountEpisodes!.Value);
        return x.Build();
    }
    
}