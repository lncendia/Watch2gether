using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Films.Infrastructure.Storage.Models.Abstractions;
using Films.Infrastructure.Storage.Models.Countries;
using Films.Infrastructure.Storage.Models.Genres;
using Films.Infrastructure.Storage.Models.Persons;
using Films.Infrastructure.Storage.Models.Playlists;

namespace Films.Infrastructure.Storage.Models.Films;

public class FilmModel : IAggregateModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public bool IsSerial { get; set; }
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
    public List<PersonModel> Screenwriters { get; set; } = [];
    
    public List<PlaylistFilmModel> Playlists { get; set; } = [];
}