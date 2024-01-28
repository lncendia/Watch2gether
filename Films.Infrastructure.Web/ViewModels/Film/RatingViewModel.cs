namespace Films.Infrastructure.Web.Models.Film;

public class RatingViewModel
{
    public RatingViewModel(double rating, int count)
    {
        Rating = rating.ToString("F2");
        Count = count;
    }

    public string Rating { get; }
    public int Count { get; }
}