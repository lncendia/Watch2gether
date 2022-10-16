namespace Watch2gether.Domain.Abstractions.Exceptions;

public class SeasonException : Exception
{
    public SeasonException() : base("There is no such season.")
    {
    }
}