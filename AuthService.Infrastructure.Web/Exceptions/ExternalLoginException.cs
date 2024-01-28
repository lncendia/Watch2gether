namespace AuthService.Infrastructure.Web.Exceptions;

/// <summary>
/// Исключение, возникающее при ошибке аутентификации с использованием внешнего провайдера.
/// </summary>
public class ExternalLoginException() : Exception("Authentication error using an external provider");