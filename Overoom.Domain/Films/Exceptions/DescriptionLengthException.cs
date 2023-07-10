namespace Overoom.Domain.Films.Exceptions;

public class DescriptionLengthException : Exception
{
    public DescriptionLengthException():base("The movie description length must be between 1 and 1000 characters")
    {
        
    }   
}