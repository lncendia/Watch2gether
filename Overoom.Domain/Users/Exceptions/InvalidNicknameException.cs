namespace Overoom.Domain.Users.Exceptions;

public class InvalidNicknameException : Exception
{
    public InvalidNicknameException() : base(
        "Nickname must be between 3 and 20 characters long and can contain only latin or cyrillic letters, digits and underscores.")
    {
    }
}