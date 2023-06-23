namespace Overoom.Application.Abstractions.Users.Interfaces;

public interface ILoginDataService
{
    Task RequestResetEmailAsync(string email, string newEmail, string resetUrl);
    Task ResetEmailAsync(string email, string newEmail, string code);
    Task ChangePasswordAsync(string email, string oldPassword, string newPassword);
}