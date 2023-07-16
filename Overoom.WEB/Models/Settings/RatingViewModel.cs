namespace Overoom.WEB.Models.Settings;

public class RatingViewModel
{
    public RatingViewModel(string name, Guid id, int year, double score, Uri poster)
    {
        Name = name + " (" + year + ")";
        Id = id;
        Score = score;
        Poster = poster;
    }

    public string Name { get; }
    public Guid Id { get; }
    public double Score { get; }
    public Uri Poster { get; }
}