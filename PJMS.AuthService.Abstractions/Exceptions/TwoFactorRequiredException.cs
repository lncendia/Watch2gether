using PJMS.AuthService.Abstractions.Entities;

namespace PJMS.AuthService.Abstractions.Exceptions;

/// <summary>
/// Исключение, возникающее при прохождении аутентификации паролем со включенной 2FA
/// </summary>
/// <param name="user">Объект Identity пользователя</param>
public class TwoFactorRequiredException(AppUser user) : Exception("Two factor authentication is required")
{
    /// <summary>
    /// Аутентифицированный пользователь
    /// </summary>
    public AppUser User { get; } = user;
}