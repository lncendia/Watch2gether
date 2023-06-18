namespace Overoom.Application.Abstractions.Films.Kinopoisk.DTOs;

public class Episode
{
    public Episode(int episodeNumber, string? name)
    {
        EpisodeNumber = episodeNumber;
        Name = name;
    }

    public int EpisodeNumber;
    public string? Name;
}