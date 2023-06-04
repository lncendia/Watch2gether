namespace Overoom.Application.Abstractions.User.Interfaces;

public interface IUserSecurityService
{
    Task RequestResetEmailAsync(string email, string newEmail, string resetUrl);
    Task ResetEmailAsync(string email, string newEmail, string code);
    Task ChangePasswordAsync(string email, string oldPassword, string newPassword);
}