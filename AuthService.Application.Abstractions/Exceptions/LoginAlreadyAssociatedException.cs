namespace AuthService.Application.Abstractions.Exceptions;

/// <summary>
/// Исключение, возникающее при существовании логина через внешний idp у другого пользователя.
/// </summary>
public class LoginAlreadyAssociatedException() : Exception("The login is associated with another user");