namespace Watch2gether.Domain.Users.Exceptions;

public class InvalidNicknameException : Exception
{
    public InvalidNicknameException(string name) : base(
        $"Bad nickname: {name}. Nickname must be between 3 and 20 characters long and can contain only latin or cyrillic letters, digits and underscores.")
    {
    }
}