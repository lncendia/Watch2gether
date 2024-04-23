namespace PJMS.AuthService.Abstractions.Exceptions;

/// <summary>
/// Исключение, возникающее, когда пароль не прошел валидацию.
/// </summary>
public class PasswordValidationException : Exception
{
    /// <summary>
    /// Словарь содержит в себе код и описание ошибки.
    /// Возможные коды ошибок:
    /// - InvalidLength: Длина пароля не соответствует требованиям.
    /// - RequiresLower: Пароль должен содержать строчные буквы.
    /// - RequiresUpper: Пароль должен содержать прописные буквы.
    /// - RequiresDigit: Пароль должен содержать цифры.
    /// - RequiresNonAlphanumeric: Пароль должен содержать специальные символы.
    /// </summary>
    public required Dictionary<string, string> ValidationErrors { get; init; }
}