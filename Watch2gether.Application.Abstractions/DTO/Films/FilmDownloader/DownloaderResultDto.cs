namespace Watch2gether.Application.Abstractions.DTO.Films.FilmDownloader;

public class DownloaderResultDto
{
    public DownloaderResultDto(List<FilmShortInfoDto> films, bool moreAvailable)
    {
        Films = films;
        MoreAvailable = moreAvailable;
    }

    public List<FilmShortInfoDto> Films { get; }
    public bool MoreAvailable { get; }
}