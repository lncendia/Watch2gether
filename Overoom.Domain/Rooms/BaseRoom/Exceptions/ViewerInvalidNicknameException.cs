namespace Overoom.Domain.Users.Exceptions;

public class ViewerInvalidNicknameException : Exception
{
    public ViewerInvalidNicknameException(string name) : base(
        $"Bad nickname: {name}. Nickname must be between 3 and 20 characters long and can contain only latin or cyrillic letters, digits and underscores.")
    {
    }
}