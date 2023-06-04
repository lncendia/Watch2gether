using Overoom.Application.Abstractions.Film.DTOs.FilmLoader;
using Overoom.Application.Abstractions.Film.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Film.Specifications;
using Overoom.Domain.Film.Specifications.Visitor;
using Overoom.Domain.Specifications;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Application.Services.Film;

public class FilmLoaderService : IFilmLoaderService
{
    private readonly IFilmInfoGetterService _filmInfoGetterService;
    private readonly IFilmPosterService _filmPosterService;
    private readonly IUnitOfWork _unitOfWork;

    public FilmLoaderService(IFilmInfoGetterService filmInfoGetterService, IFilmPosterService filmPosterService,
        IUnitOfWork unitOfWork)
    {
        _filmInfoGetterService = filmInfoGetterService;
        _filmPosterService = filmPosterService;
        _unitOfWork = unitOfWork;
    }

    public async Task<DownloaderResultDto> GetAsync(string? title, int page)
    {
        var data = await _filmInfoGetterService.GetAsync(title, page, 20);
        return new DownloaderResultDto(
            data.Films.Select(x => new FilmShortInfoDto(x.Id, x.Name, x.Year, x.Type)).ToList(),
            data.LastPage > page);
    }

    public async Task DownloadAsync(int id)
    {
        var filmInfo = await _filmInfoGetterService.GetFromVideoCdnIdAsync(id);
        ISpecification<Domain.Film.Entities.Film, IFilmSpecificationVisitor> spec = new FilmByNameSpecification(filmInfo.Name);
        spec = new AndSpecification<Domain.Film.Entities.Film, IFilmSpecificationVisitor>(spec,
            new FilmByYearsSpecification(filmInfo.Date.Year, filmInfo.Date.Year));
        var film = (await _unitOfWork.FilmRepository.Value.FindAsync(spec, take: 1)).FirstOrDefault();

        if (film != null)
        {
            film.UpdateInfo(filmInfo.Description, filmInfo.ShortDescription, filmInfo.Rating, filmInfo.CountSeasons,
                filmInfo.CountEpisodes);
            await _unitOfWork.FilmRepository.Value.UpdateAsync(film);
        }
        else
        {
            var avatar = await _filmPosterService.SaveAsync(filmInfo.AvatarUrl);

            film = new Domain.Film.Entities.Film(filmInfo.Name, filmInfo.Description, filmInfo.ShortDescription, filmInfo.Date,
                filmInfo.Rating, filmInfo.Type, filmInfo.Url, filmInfo.Genres,
                filmInfo.Actors.Where(x => !string.IsNullOrEmpty(x.description)).Take(10),
                filmInfo.Directors.Take(5),
                filmInfo.Screenwriters.Take(5), filmInfo.Countries, avatar, filmInfo.CountSeasons,
                filmInfo.CountEpisodes);
            await _unitOfWork.FilmRepository.Value.AddAsync(film);
        }

        await _unitOfWork.SaveAsync();
    }
}