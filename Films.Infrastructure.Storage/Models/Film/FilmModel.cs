using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Films.Domain.Films.Enums;
using Films.Infrastructure.Storage.Models.Abstractions;
using Films.Infrastructure.Storage.Models.Country;
using Films.Infrastructure.Storage.Models.Genre;
using Films.Infrastructure.Storage.Models.Person;

namespace Films.Infrastructure.Storage.Models.Film;

public class FilmModel : IAggregateModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public FilmType Type { get; set; }
    [MaxLength(200)] public string Title { get; set; } = null!;
    public Uri PosterUrl { get; set; } = null!;
    [MaxLength(1500)] public string Description { get; set; } = null!;
    [MaxLength(500)] public string? ShortDescription { get; set; }
    public int Year { get; set; }
    public double? RatingKp { get; set; }
    public double? RatingImdb { get; set; }
    public double UserRating { get; set; }
    public int UserRatingsCount { get; set; }

    public int? CountSeasons { get; set; }
    public int? CountEpisodes { get; set; }


    public List<CdnModel> CdnList { get; set; } = [];
    public List<GenreModel> Genres { get; set; } = [];
    public List<CountryModel> Countries { get; set; } = [];
    public List<FilmActorModel> Actors { get; set; } = [];
    public List<PersonModel> Directors { get; set; } = [];
    public List<PersonModel> ScreenWriters { get; set; } = [];
}