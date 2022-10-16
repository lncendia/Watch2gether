namespace Watch2gether.Application.Abstractions.Exceptions.Rooms;

public class InvalidVideoUrlException : Exception
{
    public InvalidVideoUrlException() : base("Invalid video url")
    {
    }
}