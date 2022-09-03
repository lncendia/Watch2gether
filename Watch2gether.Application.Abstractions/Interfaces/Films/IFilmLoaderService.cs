using Watch2gether.Application.Abstractions.DTO.Films.FilmDownloader;

namespace Watch2gether.Application.Abstractions.Interfaces.Films;

public interface IFilmLoaderService
{
    Task<DownloaderResultDto> GetFilmsAsync(string? title, int page);
    Task DownloadFilmAsync(int id);
}