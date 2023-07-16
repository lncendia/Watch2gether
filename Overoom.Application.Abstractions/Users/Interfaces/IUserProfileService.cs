using Overoom.Application.Abstractions.Users.DTOs;

namespace Overoom.Application.Abstractions.Users.Interfaces;

public interface IUserProfileService
{

    Task<ProfileDto> GetProfileAsync(Guid id);
    Task<List<RatingDto>> GetRatingsAsync(Guid id, int page);
    Task ChangeNameAsync(Guid id, string name);
    Task<Uri> ChangeAvatarAsync(Guid id, Stream avatar);
    Task RequestResetEmailAsync(Guid id, string newEmail, string resetUrl);
    Task ResetEmailAsync(Guid id, string newEmail, string code);
    Task ChangePasswordAsync(Guid id, string oldPassword, string newPassword);
}