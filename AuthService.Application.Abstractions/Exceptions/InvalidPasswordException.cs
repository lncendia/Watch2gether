namespace AuthService.Application.Abstractions.Exceptions;

/// <summary>
/// Иключение, возникающее при неверно введенем пароле.
/// </summary>
public class InvalidPasswordException : Exception
{
    public InvalidPasswordException() : base("Invalid password entered")
    {
    }
}