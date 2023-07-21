namespace Overoom.Domain.Users.Exceptions;

public class NicknameFormatException : Exception
{
    public NicknameFormatException() : base(
        "Nickname must contain only latin or cyrillic letters, digits, spaces and underscores")
    {
    }
}