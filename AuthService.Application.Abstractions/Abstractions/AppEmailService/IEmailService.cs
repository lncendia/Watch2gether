using AuthService.Application.Abstractions.Abstractions.AppEmailService.Structs;

namespace AuthService.Application.Abstractions.Abstractions.AppEmailService;

/// <summary>
/// Интерфейс отправки Email
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Метод отправляет Email
    /// </summary>
    /// <param name="emailData">Объект данных об отправляемом Email</param>
    Task SendAsync(EmailData emailData);
}