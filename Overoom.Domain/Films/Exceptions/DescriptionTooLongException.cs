namespace Overoom.Domain.Films.Exceptions;

public class DescriptionTooLongException : Exception
{
    public DescriptionTooLongException():base("The movie description cannot be longer than 1500 characters")
    {
        
    }   
}