using Films.Domain.Films.Enums;

namespace Films.Infrastructure.Web.Models.Rooms.FilmRoom;

public class FilmViewModel
{
    public FilmViewModel(string name, Uri url, FilmType type, CdnType cdnType)
    {
        Name = name;
        Url = url;
        Type = type;
        CdnType = cdnType;
    }

    public string Name { get; }
    public Uri Url { get; }
    public CdnType CdnType { get; }
    public FilmType Type { get; }
}