using Overoom.Domain.Films.Enums;
using Overoom.Infrastructure.Storage.Models.Abstractions;
using Overoom.Infrastructure.Storage.Models.Country;
using Overoom.Infrastructure.Storage.Models.Genre;
using Overoom.Infrastructure.Storage.Models.Person;

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
    public List<FilmActorModel> Actors { get; set; } = new();
    public List<PersonModel> Directors { get; set; } = new();
    public List<PersonModel> ScreenWriters { get; set; } = new();
}