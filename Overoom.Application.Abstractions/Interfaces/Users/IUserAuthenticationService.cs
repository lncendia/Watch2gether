using Microsoft.AspNetCore.Identity;
using Overoom.Application.Abstractions.DTO.Users;
using Overoom.Application.Abstractions.Entities.User;

namespace Overoom.Application.Abstractions.Interfaces.Users;

public interface IUserAuthenticationService
{
    Task CreateAsync(UserDto userDto, string confirmUrl);
    Task<UserData> AuthenticateAsync(string username, string password);
    Task ResetPasswordAsync(string email, string code, string newPassword);
    Task RequestResetPasswordAsync(string email, string resetUrl);
    Task<UserData> AcceptCodeAsync(string userId, string code);
    Task<UserData> ExternalLoginAsync(ExternalLoginInfo info);
}