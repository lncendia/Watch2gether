namespace Overoom.Application.Abstractions.Profile.DTOs;

public class RatingDto
{
    public RatingDto(string name, Guid id, int year, double score, Uri poster)
    {
        Name = name;
        Id = id;
        Year = year;
        Score = score;
        Poster = poster;
    }

    public string Name { get; }
    public Guid Id { get; }
    public int Year { get; }
    public double Score { get; }
    public Uri Poster { get; }
}