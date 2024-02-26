namespace PJMS.AuthService.Abstractions.Exceptions;

/// <summary>
/// Исключение, возникающее, когда 2FA выключена.
/// </summary>
public class TwoFactorNotEnabledException() : Exception("2FA not enabled");