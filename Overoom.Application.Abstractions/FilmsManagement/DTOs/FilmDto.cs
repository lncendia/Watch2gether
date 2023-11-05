namespace Overoom.Application.Abstractions.FilmsManagement.DTOs;

public class FilmDto
{
    public FilmDto(Guid filmId, string description, string shortDescription, double rating,
        IReadOnlyCollection<CdnDto> cdnList, int? countSeasons, int? countEpisodes, string title)
    {
        CountSeasons = countSeasons;
        CountEpisodes = countEpisodes;
        Title = title;
        FilmId = filmId;
        CdnList = cdnList;
        Description = description;
        Rating = rating;
        ShortDescription = shortDescription;
    }

    public Guid FilmId { get; }
    public string Title { get; }
    public string Description { get; }
    public string ShortDescription { get; }
    public double Rating { get; }
    public int? CountSeasons { get; }
    public int? CountEpisodes { get; }
    public IReadOnlyCollection<CdnDto> CdnList { get; }
}