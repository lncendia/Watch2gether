namespace Overoom.Domain.Film.ValueObject;

public class FilmInfo
{
    public FilmInfo(double rating, string description, string? shortDescription, int? countSeasons, int? countEpisodes)
    {
        RatingKp = rating;
        Description = description;
        CountSeasons = countSeasons;
        CountEpisodes = countEpisodes;
        ShortDescription = shortDescription;
    }
    
    public string Description { get; }
    public string? ShortDescription { get; }
    public int? CountSeasons { get; }
    public int? CountEpisodes { get; }

    private readonly double _rating;

    public double RatingKp
    {
        get => _rating;
        private init
        {
            if (value is < 0 or > 10)
                throw new ArgumentException("Rating must be between 0 and 10");
            _rating = value;
        }
    }
}