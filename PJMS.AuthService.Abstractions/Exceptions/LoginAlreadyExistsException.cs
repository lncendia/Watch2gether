namespace PJMS.AuthService.Abstractions.Exceptions;

/// <summary>
/// Исключение, возникающее при существовании логина через внешний idp у этого пользователя.
/// </summary>
public class LoginAlreadyExistsException() : Exception("The user already has the username of this provider");