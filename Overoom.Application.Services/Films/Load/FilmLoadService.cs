using Overoom.Application.Abstractions.Films.Catalog.Exceptions;
using Overoom.Application.Abstractions.Films.Load.DTOs;
using Overoom.Application.Abstractions.Films.Load.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Films.Entities;
using CdnDto = Overoom.Domain.Films.DTOs.CdnDto;

namespace Overoom.Application.Services.Films.Load;

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
        Uri? poster = null;
        if (film.PosterUri != null) poster = await _filmPosterService.SaveAsync(film.PosterUri);
        else if (film.PosterStream != null) poster = await _filmPosterService.SaveAsync(film.PosterStream);
        else throw new ArgumentException(""); //todo: exception
        var builder = FilmBuilder.Create()
            .WithName(film.Name)
            .WithDescription(film.Description)
            .WithYear(film.Year)
            .WithRating(film.Rating)
            .WithType(film.Type)
            .WithCdn(film.CdnList.Select(x => new CdnDto(x.Type, x.Uri, x.Quality, x.Voices)))
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
        if (!string.IsNullOrEmpty(filmToChange.Description)) film.Description = filmToChange.Description;
        if (!string.IsNullOrEmpty(filmToChange.ShortDescription)) film.ShortDescription = filmToChange.ShortDescription;
        if (filmToChange.Rating.HasValue) film.Rating = filmToChange.Rating.Value;
        if (filmToChange is { CountSeasons: not null, CountEpisodes: not null })
            film.UpdateSeriesInfo(filmToChange.CountSeasons.Value, filmToChange.CountEpisodes.Value);


        if (filmToChange.PosterUri != null)
        {
            await _filmPosterService.DeleteAsync(film.PosterUri);
            var poster = await _filmPosterService.SaveAsync(film.PosterUri);
            film.PosterUri = poster;
        }

        if (filmToChange.CdnList != null)
        {
            foreach (var cdnDto in filmToChange.CdnList)
            {
                film.AddOrChangeCdn(new CdnDto(cdnDto.Type, cdnDto.Uri, cdnDto.Quality, cdnDto.Voices));
            }
        }

        await _unitOfWork.FilmRepository.Value.UpdateAsync(film);
        await _unitOfWork.SaveChangesAsync();
    }
}