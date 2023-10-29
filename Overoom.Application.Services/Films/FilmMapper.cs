using Overoom.Application.Abstractions.Movie.DTOs;
using Overoom.Application.Abstractions.Movie.Interfaces;
using Overoom.Domain.Films.Enums;
using Overoom.Domain.Ratings.Entities;
using Overoom.Domain.Users.Entities;
using Overoom.Domain.Films.Entities;

namespace Overoom.Application.Services.Films;

/// <summary> 
/// Представляет класс, отвечающий за отображение объектов фильма. 
/// </summary> 
public class FilmMapper : IFilmMapper
{
    /// <summary> 
    /// Преобразует объект фильма, рейтинга и пользователя в объект DTO фильма. 
    /// </summary> 
    /// <param name="film">Объект фильма.</param> 
    /// <param name="rating">Рейтинг фильма (необязательно).</param> 
    /// <param name="user">Пользователь (необязательно).</param> 
    /// <returns>DTO фильма.</returns>
    public FilmDto Map(Film film, Rating? rating, User? user)
    {
        // Создаем Builder для DTO
        var x = FilmDtoBuilder.Create() 
            .WithId(film.Id) // Устанавливаем идентификатор фильма 
            .WithName(film.Name) // Устанавливаем название фильма 
            .WithYear(film.Year) // Устанавливаем год выпуска фильма 
            .WithType(film.Type) // Устанавливаем тип фильма 
            .WithPoster(film.PosterUri) // Устанавливаем ссылку на постер фильма 
            .WithDescription(film.Description) // Устанавливаем описание фильма 
            .WithRating(film.Rating) // Устанавливаем рейтинг фильма 
            .WithUserRating(film.UserRating, film.UserRatingsCount) // Устанавливаем пользовательский рейтинг фильма 
            .WithDirectors(film.FilmTags.Directors) // Устанавливаем режиссеров фильма 
            .WithScreenwriters(film.FilmTags.Screenwriters) // Устанавливаем сценаристов фильма 
            .WithGenres(film.FilmTags.Genres) // Устанавливаем жанры фильма 
            .WithCountries(film.FilmTags.Countries) // Устанавливаем страны производства фильма 
            .WithActors(film.FilmTags.Actors.Select(x => (x.ActorName, x.ActorDescription)).ToList()) // Устанавливаем актеров фильма 
            .WithCdn(film.CdnList.Select(x => new CdnDto(x.Type, x.Quality, x.Voices)).ToList()); // Устанавливаем информацию о CDN фильма 
        
        // Если есть рейтинг пользователя, устанавливаем его оценку 
        if (rating != null) x = x.WithUserScore(rating.Score);
        
        // Если есть пользователь, проверяем, есть ли фильм в его списке просмотра 
        if (user != null) x = x.WithWatchlist(user.Watchlist.Any(note => note.FilmId == film.Id));
        
        // Если тип фильма - сериал
        if (film.Type == FilmType.Serial)
            
            // Устанавливаем количество сезонов и эпизодов сериала 
            x = x.WithEpisodes(film.CountSeasons!.Value, film.CountEpisodes!.Value);
        
        // Возвращаем построенный объект DTO фильма
        return x.Build();
    }
}