using Overoom.Domain.Films.Enums;
using Overoom.Infrastructure.Storage.Models.Abstractions;

namespace Overoom.Infrastructure.Storage.Models.Film;

public class FilmModel : IAggregateModel
{
    public Guid Id { get; set; }
    public FilmType Type { get; set; }
    public string Name { get; set; } = null!;
    public string NameNormalized { get; set; } = null!;
    public Uri PosterUri { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? ShortDescription { get; set; }
    public int Year { get; set; }
    public double Rating { get; set; }
    public double UserRating { get; set; }
    public int UserRatingsCount { get; set; }

    public int? CountSeasons { get; set; }
    public int? CountEpisodes { get; set; }


    public List<CdnModel> CdnList { get; set; } = new();
    public List<GenreModel> Genres { get; set; } = new();
    public List<CountryModel> Countries { get; set; } = new();
    public List<ActorModel> Actors { get; set; } = new();
    public List<DirectorModel> Directors { get; set; } = new();
    public List<ScreenWriterModel> ScreenWriters { get; set; } = new();
}