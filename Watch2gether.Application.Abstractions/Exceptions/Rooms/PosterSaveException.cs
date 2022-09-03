namespace Watch2gether.Application.Abstractions.Exceptions.Rooms;

public class PosterSaveException : Exception
{
    public PosterSaveException(Exception ex) : base("Failed to save poster", ex)
    {
    }
}