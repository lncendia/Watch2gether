using PJMS.AuthService.Abstractions.AppEmailService.Structs;

namespace PJMS.AuthService.Abstractions.AppEmailService;

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