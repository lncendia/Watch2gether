namespace Overoom.Application.Abstractions.MovieApi.DTOs;

public class Episode
{
    public Episode(int episodeNumber, DateOnly? releaseDate)
    {
        EpisodeNumber = episodeNumber;
        ReleaseDate = releaseDate;
    }

    public int EpisodeNumber { get; }
    public DateOnly? ReleaseDate { get; }
}