using Overoom.Application.Abstractions.Film.Catalog.Exceptions;
using Overoom.Application.Abstractions.Film.Load.DTOs;
using Overoom.Application.Abstractions.Film.Load.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Film.Entities;

namespace Overoom.Application.Services.Film.Load;

public class FilmLoadService : IFilmLoadService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFilmPosterService _filmPosterService;

    public FilmLoadService(IUnitOfWork unitOfWork, IFilmPosterService filmPosterService)
    {
        _unitOfWork = unitOfWork;
        _filmPosterService = filmPosterService;
    }

    public async Task LoadAsync(FilmLoadDto film)
    {
        var poster = await _filmPosterService.SaveAsync(film.PosterUri);
        var builder = FilmBuilder.Create()
            .WithName(film.Name)
            .WithDescription(film.Description)
            .WithYear(film.Year)
            .WithRating(film.RatingKp)
            .WithType(film.Type)
            .WithCdn(film.CdnList.Select(x => new Domain.Film.DTOs.CdnDto(x.Type, x.Uri, x.Quality, x.Voices)))
            .WithGenres(film.Genres)
            .WithActors(film.Actors)
            .WithDirectors(film.Directors)
            .WithScreenwriters(film.Screenwriters)
            .WithCountries(film.Countries)
            .WithPoster(poster);
        if (!string.IsNullOrEmpty(film.ShortDescription)) builder = builder.WithShortDescription(film.ShortDescription);
        if (film is { CountSeasons: not null, CountEpisodes: not null })
            builder = builder.WithEpisodes(film.CountSeasons.Value, film.CountEpisodes.Value);
        await _unitOfWork.FilmRepository.Value.AddAsync(builder.Build());
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ChangeAsync(FilmChangeDto filmToChange)
    {
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(filmToChange.FilmId);
        if (film == null) throw new FilmNotFoundException();
        await _mapper.MapAsync(film, filmToChange);
        await _unitOfWork.FilmRepository.Value.UpdateAsync(film);
        await _unitOfWork.SaveChangesAsync();
    }
}