namespace Overoom.Application.Abstractions.Films.Load.DTOs;

public class FilmChangeDto
{
    public FilmChangeDto(Guid filmId, string? description, string? shortDescription, Uri? posterUri, double? rating,
        IReadOnlyCollection<CdnDto>? cdnList, int? countSeasons, int? countEpisodes)
    {
        CountSeasons = countSeasons;
        CountEpisodes = countEpisodes;
        PosterUri = posterUri;
        FilmId = filmId;
        CdnList = cdnList;
        Description = description;
        Rating = rating;
        ShortDescription = shortDescription;
    }

    public Guid FilmId { get; }
    public string? Description { get; }
    public string? ShortDescription { get; }
    public Uri? PosterUri { get; }
    public double? Rating { get; }
    public int? CountSeasons { get; }
    public int? CountEpisodes { get; }
    public IReadOnlyCollection<CdnDto>? CdnList { get; }
}