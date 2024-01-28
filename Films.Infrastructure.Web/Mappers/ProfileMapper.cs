using Films.Application.Abstractions.Profile.DTOs;
using Films.Infrastructure.Web.Mappers.Abstractions;
using Films.Infrastructure.Web.Models.Settings;

namespace Films.Infrastructure.Web.Mappers;

public class ProfileMapper : IProfileMapper
{
    public ProfileViewModel Map(ProfileDto dto, IReadOnlyCollection<string> genres)
    {
        var watchedFilms = dto.WatchedFilms.Select(x => new FilmViewModel(x.Name, x.Id, x.Year, x.Poster)).ToList();
        var favoriteFilms = dto.FavoriteFilms.Select(x => new FilmViewModel(x.Name, x.Id, x.Year, x.Poster)).ToList();
        var allows = new AllowsViewModel(dto.Allows.Beep, dto.Allows.Scream, dto.Allows.Change);
        return new ProfileViewModel(dto.Name, dto.Email, dto.Avatar, watchedFilms, favoriteFilms, genres, allows);
    }

    public RatingViewModel Map(RatingDto dto) => new(dto.Name, dto.Id, dto.Year, dto.Score, dto.Poster);
}