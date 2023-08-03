namespace Overoom.Application.Abstractions.MovieApi.DTOs;

public class Season
{
    public Season(int number, IReadOnlyCollection<Episode> episodes)
    {
        Number = number;
        Episodes = episodes;
    }

    public int Number { get; }
    public IReadOnlyCollection<Episode> Episodes { get; }
}