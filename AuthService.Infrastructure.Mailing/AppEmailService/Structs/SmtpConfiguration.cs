namespace AuthService.Infrastructure.Mailing.AppEmailService.Structs;

/// <summary>
/// Объект данных SMTP настроек для Email 
/// </summary>
public class SmtpConfiguration
{
    /// <summary>
    /// SMTP хост
    /// </summary>
    public required string Host { get; init; }

    /// <summary>
    /// SMTP порт
    /// </summary>
    public required int Port { get; init; }

    /// <summary>
    /// SMTP логин
    /// </summary>
    public required string Login { get; init; }

    /// <summary>
    /// SMTP пароль
    /// </summary>
    public required string Password { get; init; }
}