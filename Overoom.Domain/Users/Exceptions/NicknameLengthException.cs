namespace Overoom.Domain.Users.Exceptions;

public class NicknameLengthException : Exception
{
    public NicknameLengthException() : base("Nickname must be between 3 and 20 characters long")
    {
    }
}