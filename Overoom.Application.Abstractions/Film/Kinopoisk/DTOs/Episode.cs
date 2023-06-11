namespace Overoom.Application.Abstractions.Film.Kinopoisk.DTOs;

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