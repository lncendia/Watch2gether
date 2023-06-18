namespace Overoom.Application.Abstractions.Users.Interfaces;

public interface IEmailService
{
    public Task SendEmailAsync(string email, string message);
}