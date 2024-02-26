namespace PJMS.AuthService.Abstractions.Exceptions;

/// <summary>
/// Исключение, возникающее при сбросе 2FA.
/// </summary>
public class TwoFactorResetException() : Exception("Error while trying remove 2FA");