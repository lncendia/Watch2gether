namespace PJMS.AuthService.Abstractions.Exceptions;

/// <summary>
/// Исключение, возникающее, когда электронная почта не подтверждена.
/// </summary>
public class EmailNotConfirmedException() : Exception("For this action, the email must be confirmed");