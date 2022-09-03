using Watch2gether.Domain.Films.Enums;

namespace Watch2gether.WEB.Models.FilmDownloader;

public class FilmDownloaderViewModel
{
    public FilmDownloaderViewModel(List<FilmInfoViewModel> films, bool moreAvailable)
    {
        MoreAvailable = moreAvailable;
        Films = films;
    }

    public List<FilmInfoViewModel> Films { get; }
    public bool MoreAvailable { get; }
}

public class FilmInfoViewModel
{
    public FilmInfoViewModel(string id, string name, int year, FilmType type)
    {
        Id = id;
        Name = name;
        Year = year;
        Type = type switch
        {
            FilmType.Serial => "Сериал",
            FilmType.Film => "Фильм",
            _ => throw new NotImplementedException()
        };
    }

    public string Id { get; }
    public string Name { get; }
    public int Year { get; }
    public string Type { get; }
}