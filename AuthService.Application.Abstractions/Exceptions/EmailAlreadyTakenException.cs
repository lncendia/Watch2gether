namespace AuthService.Application.Abstractions.Exceptions;

/// <summary>
/// Исключение, возникающее при попытке взятие уже зарегистрированного email.
/// </summary>
public class EmailAlreadyTakenException() : Exception("The user is already registered");