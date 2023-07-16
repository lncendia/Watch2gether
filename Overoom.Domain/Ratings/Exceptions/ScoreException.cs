namespace Overoom.Domain.Ratings.Exceptions;

public class ScoreException : Exception
{
    public ScoreException() : base("Rating must be between 0 and 10")
    {
    }
}