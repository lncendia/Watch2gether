using PJMS.AuthService.Abstractions.Enums;

namespace PJMS.AuthService.Web.TwoFactor.ViewModels;

/// <summary>
/// Модель представления для прохождения 2FA
/// </summary>
public class LoginTwoStepViewModel
{
    /// <summary>
    /// Необходимо ли отображать вариант получения кода по почте (false - если почта не подтверждена)
    /// </summary>
    public bool NeedShowEmail { get; init; }
    
    /// <summary>
    /// Флаг необходимости запомнить пользователя
    /// </summary>
    public bool RememberMe { get; init; }

    /// <summary>
    /// Откуда код
    /// </summary>
    public CodeType CodeType { get; init; }

    /// <summary>
    /// Url адрес возврата после прохождения 2FA
    /// </summary>
    public string ReturnUrl { get; init; } = "/";
    
    /// <summary>
    /// Сообщение для пользователя
    /// </summary>
    public string? Message { get; init; }
}