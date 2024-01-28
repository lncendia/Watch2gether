namespace AuthService.Application.Abstractions.Exceptions;

/// <summary>
/// Исключение, возникающее при недопустимом формате электронной почты.
/// </summary>
public class EmailFormatException() : Exception("Email must be in format: <user>@<domain>");