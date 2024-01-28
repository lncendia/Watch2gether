namespace AuthService.Application.Abstractions.Exceptions;

/// <summary>
/// Исключение, возникающее при ошибке отправки электронной почты.
/// </summary>
/// <param name="innerException">Внутреннее исключение.</param>
public class EmailSendException(Exception innerException) : Exception("Failed to send email.", innerException);