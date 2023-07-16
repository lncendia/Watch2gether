namespace Overoom.Application.Abstractions.Movie.DTOs;

public class RatingDto
{
    public RatingDto(double rating, int count)
    {
        Rating = rating;
        Count = count;
    }

    public double Rating { get; }
    public int Count { get; }

}