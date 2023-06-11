namespace Overoom.Application.Abstractions.User.Interfaces;

public interface IEmailService
{
    public Task SendEmailAsync(string email, string message);
}