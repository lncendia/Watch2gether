using Microsoft.AspNetCore.Identity;
using Watch2gether.Application.Abstractions.DTO.Users;
using Watch2gether.Application.Abstractions.Entities.User;

namespace Watch2gether.Application.Abstractions.Interfaces.Users;

public interface IUserAuthenticationService
{
    Task CreateAsync(UserDto userDto, string confirmUrl);
    Task<UserData> AuthenticateAsync(string username, string password);
    Task ResetPasswordAsync(string email, string code, string newPassword);
    Task RequestResetPasswordAsync(string email, string resetUrl);
    Task<UserData> AcceptCodeAsync(string userId, string code);
    Task<UserData> ExternalLoginAsync(ExternalLoginInfo info);
}