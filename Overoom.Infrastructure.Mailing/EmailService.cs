using System.Net;
using System.Net.Mail;
using Overoom.Application.Abstractions.User.Interfaces;

namespace Overoom.Infrastructure.Mailing;

public class EmailService : IEmailService
{
    private readonly SmtpClient _client;

    public EmailService(string login, string password, string host, int port)
    {
        _client = new SmtpClient
        {
            Host = host,
            Port = port,
            EnableSsl = true,
            Credentials = new NetworkCredential(login, password)
        };
    }

    public Task SendAsync(string email, string message)
    {
        var mail = new MailMessage();
        //TODO: check
        mail.To.Add(email);
        mail.Body = message;
        mail.IsBodyHtml = true;
        return _client.SendMailAsync(mail);
    }
}