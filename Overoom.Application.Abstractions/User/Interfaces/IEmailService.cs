namespace Overoom.Application.Abstractions.User.Interfaces;

public interface IEmailService
{
    public Task SendAsync(string email, string message);
}