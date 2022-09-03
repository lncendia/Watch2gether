namespace Watch2gether.Application.Abstractions.Interfaces.Users;

public interface IEmailService
{
    public Task SendEmailAsync(string email, string message);
}