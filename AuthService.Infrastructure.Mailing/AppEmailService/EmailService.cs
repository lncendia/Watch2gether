using System.Net;
using System.Net.Mail;
using AuthService.Application.Abstractions.Abstractions.AppEmailService;
using AuthService.Application.Abstractions.Abstractions.AppEmailService.Structs;
using AuthService.Application.Abstractions.Exceptions;
using AuthService.Infrastructure.Mailing.AppEmailService.Structs;
using Microsoft.Extensions.Localization;
using PJMS.AuthService.Mail.AppEmailService;

namespace AuthService.Infrastructure.Mailing.AppEmailService;

/// <inheritdoc />
/// <summary>
/// Реализация интерфейс отправки Email
/// </summary>
public class EmailService : IEmailService
{

    private readonly SmtpClient _client;
    private readonly EmailTemplateConfiguration _templateConfiguration;
    private readonly IStringLocalizer<EmailService> _localizer;
    
    /// <summary>
    /// Реализация интерфейс отправки Email
    /// </summary>
    /// <param name="smtpConfiguration">Конфигурация SMTP</param>
    /// <param name="localizer">Локализатор</param>
    /// <param name="templateConfiguration">Настройки шаблона письма</param>
    public EmailService(EmailTemplateConfiguration templateConfiguration, SmtpConfiguration smtpConfiguration, IStringLocalizer<EmailService> localizer)
    {
        _templateConfiguration = templateConfiguration;
        _localizer = localizer;
        _client = new SmtpClient
        {
            Host = smtpConfiguration.Host,
            Port = smtpConfiguration.Port,
            EnableSsl = true,
            Credentials = new NetworkCredential(smtpConfiguration.Login, smtpConfiguration.Password)
        };
    }


    /// <inheritdoc />
    /// <summary>
    /// Метод отправляет Email
    /// </summary>
    /// <param name="emailData">Объект данных об отправляемом Email</param>
    public Task SendAsync(EmailData emailData)
    {
        // Создаем посетителя
        var visitor = new EmailContentVisitor(_templateConfiguration, _localizer);
        
        // Посещаем письмо
        emailData.Accept(visitor);
        
        // Отправляем Email, контент берем из посетителя
        return SendEmailBySmtpAsync(emailData.Recipient, visitor.Subject!, visitor.HtmlContent!);
    }

    /// <summary>
    /// Метод отправляет Email через API MailGun
    /// </summary>
    /// <param name="recipient">Email получателя</param>
    /// <param name="subject">Тема письма</param>
    /// <param name="htmlContent">HTML контент письма</param>
    private async Task SendEmailBySmtpAsync(string recipient, string subject, string htmlContent)
    {
        try
        {
            var mail = new MailMessage();
            mail.To.Add(recipient);
            mail.Subject = subject;
            mail.Body = htmlContent;
            mail.IsBodyHtml = true;
            await _client.SendMailAsync(mail);
        }
        catch (Exception exception)
        {
            // Инкапсулируем полученное исключение в EmailException и вызываем его дальше
            throw new EmailSendException(exception);
        }
    }
}