namespace Overoom.Application.Abstractions.Common.Interfaces;

public interface IEmailService
{
    public Task SendEmailAsync(string email, string message);
}