using Overoom.Domain.Film.Enums;
using Overoom.Infrastructure.Storage.Models.Abstractions;

namespace Overoom.Infrastructure.Storage.Models.Films;

public class FilmModel : IAggregateModel
{
    public Guid Id { get; set; }
    public FilmType Type { get; set; }
    public string Name { get; set; } = null!;
    public string PosterUri { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? ShortDescription { get; set; }
    public DateOnly Date { get; set; }
    public double RatingKp { get; set; }
    public double UserRating { get; set; }

    public int? CountSeasons { get; set; }
    public int? CountEpisodes { get; set; }


    public List<CdnModel> CdnList { get; set; } = new();
    public List<GenreModel> Genres { get; set; } = new();
    public List<CountryModel> Countries { get; set; } = new();
    public List<ActorModel> Actors { get; set; } = new();
    public List<DirectorModel> Directors { get; set; } = new();
    public List<ScreenWriterModel> ScreenWriters { get; set; } = new();
}