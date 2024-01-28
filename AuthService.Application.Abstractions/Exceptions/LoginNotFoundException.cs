namespace AuthService.Application.Abstractions.Exceptions;

/// <summary>
/// Исключение, возникающее при отсутствии логина через внешний idp.
/// </summary>
public class LoginNotFoundException() : Exception("Login not found");