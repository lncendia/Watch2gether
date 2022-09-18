using Watch2gether.Application.Abstractions.DTO.Films.FilmDownloader;
using Watch2gether.Application.Abstractions.Interfaces.Films;
using Watch2gether.Domain.Abstractions.Repositories.UnitOfWorks;
using Watch2gether.Domain.Films;

namespace Watch2gether.Application.Services.Services.Films;

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

    public async Task<DownloaderResultDto> GetFilmsAsync(string? title, int page)
    {
        var data = await _filmInfoGetterService.GetFilmsAsync(title, page, 20);
        return new DownloaderResultDto(data.Films.Select(x => new FilmShortInfoDto(x.Id, x.Name, x.Year, x.Type)).ToList(),
            data.LastPage > page);
    }

    public async Task DownloadFilmAsync(int id)
    {
        var filmInfo = await _filmInfoGetterService.GetFilmFromVideoCdnIdAsync(id);
        var avatar = await _filmPosterService.SaveAsync(filmInfo.AvatarUrl);

        var film = new Film(filmInfo.Name, filmInfo.Description, filmInfo.ShortDescription, filmInfo.Year,
            filmInfo.Rating, filmInfo.Type, filmInfo.Url, filmInfo.Genres, filmInfo.Actors.Take(10),
            filmInfo.Directors.Take(5),
            filmInfo.Screenwriters.Take(5), filmInfo.Countries, avatar, filmInfo.CountSeasons, filmInfo.CountEpisodes);
        await _unitOfWork.FilmRepository.Value.AddAsync(film);
        await _unitOfWork.SaveAsync();
    }
}