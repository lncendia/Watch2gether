using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Overoom.Domain.Films.Enums;
using Overoom.Infrastructure.Storage.Models.Abstractions;
using Overoom.Infrastructure.Storage.Models.Country;
using Overoom.Infrastructure.Storage.Models.Genre;
using Overoom.Infrastructure.Storage.Models.Person;

namespace Overoom.Infrastructure.Storage.Models.Film;

public class FilmModel : IAggregateModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    public FilmType Type { get; set; }
    [MaxLength(200)] public string Name { get; set; } = null!;
    public Uri PosterUri { get; set; } = null!;
    [MaxLength(1500)] public string Description { get; set; } = null!;
    [MaxLength(500)] public string? ShortDescription { get; set; }
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