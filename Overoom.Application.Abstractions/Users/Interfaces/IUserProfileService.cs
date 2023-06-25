namespace Overoom.Application.Abstractions.Users.Interfaces;

public interface IUserProfileService
{
    Task ChangeNameAsync(Guid id, string name);
    Task<Uri> ChangeAvatarAsync(Guid id, Stream avatar);
    Task RequestResetEmailAsync(Guid id, string newEmail, string resetUrl);
    Task ResetEmailAsync(Guid id, string newEmail, string code);
    Task ChangePasswordAsync(Guid id, string oldPassword, string newPassword);
}