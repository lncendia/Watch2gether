using Overoom.Application.Abstractions.Users.DTOs;
using Overoom.Application.Abstractions.Users.Entities;
using Microsoft.AspNetCore.Identity;

namespace Overoom.Application.Abstractions.Users.Interfaces;

public interface IUserAuthenticationService
{
    Task CreateAsync(UserCreateDto userDto, string confirmUrl);
    Task<UserData> PasswordAuthenticateAsync(string email, string password);
    Task ResetPasswordAsync(string email, string code, string newPassword);
    Task RequestResetPasswordAsync(string email, string resetUrl);
    Task<UserData> CodeAuthenticateAsync(string userId, string code);
    Task<UserData> ExternalAuthenticateAsync(ExternalLoginInfo info);
}