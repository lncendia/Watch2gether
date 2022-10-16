namespace Watch2gether.Domain.Abstractions.Exceptions;

public class SeriesException : Exception
{
    public SeriesException() : base("There is no such series.")
    {
    }
}