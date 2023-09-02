namespace Overoom.Application.Abstractions.Profile.Interfaces;

public interface ISettingsService
{
    Task ChangeNameAsync(Guid id, string name);
    Task<Uri> ChangeAvatarAsync(Guid id, Stream avatar);
    Task RequestResetEmailAsync(Guid id, string newEmail, string resetUrl);
    Task ResetEmailAsync(Guid id, string newEmail, string code);
    Task ChangePasswordAsync(Guid id, string? oldPassword, string newPassword);
    Task ChangeAllowsAsync(Guid id, bool beep, bool scream, bool change);
}