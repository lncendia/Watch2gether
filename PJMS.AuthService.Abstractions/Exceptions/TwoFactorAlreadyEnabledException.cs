namespace PJMS.AuthService.Abstractions.Exceptions;

/// <summary>
/// Исключение, возникающее при попытке подключения 2FA, когда она уже подключена
/// </summary>
public class TwoFactorAlreadyEnabledException() : Exception("2FA already enabled.");