namespace PJMS.AuthService.Abstractions.Enums;

/// <summary>
/// Перечисление возможных типов кодов для прохождения 2FA 
/// </summary>
public enum CodeType
{
    /// <summary>
    /// Код аутентификатора
    /// </summary>
    Authenticator = 0,
    
    /// <summary>
    /// Код от EmailProvider
    /// </summary>
    Email = 1,
    
    /// <summary>
    /// Код восстановления от 2FA
    /// </summary>
    RecoveryCode = 2
}