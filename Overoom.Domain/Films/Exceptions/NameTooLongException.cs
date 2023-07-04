namespace Overoom.Domain.Films.Exceptions;

public class NameTooLongException : Exception
{
    public NameTooLongException():base("The movie name cannot be longer than 200 characters")
    {
        
    }   
}