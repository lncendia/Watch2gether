using Overoom.Application.Abstractions.Users.DTOs;
using Overoom.WEB.Mappers.Abstractions;
using Overoom.WEB.Models.Settings;

namespace Overoom.WEB.Mappers;

public class ProfileMapper : IProfileMapper
{
    public ProfileViewModel Map(ProfileDto dto)
    {
        var watchedFilms = dto.WatchedFilms.Select(x => new FilmViewModel(x.Name, x.Id, x.Year, x.Poster)).ToList();
        var favoriteFilms = dto.FavoriteFilms.Select(x => new FilmViewModel(x.Name, x.Id, x.Year, x.Poster)).ToList();
        return new ProfileViewModel(dto.Name, dto.Email, dto.Avatar, watchedFilms, favoriteFilms);
    }

    public RatingViewModel Map(RatingDto dto) => new(dto.Name, dto.Id, dto.Year, dto.Score, dto.Poster);
}