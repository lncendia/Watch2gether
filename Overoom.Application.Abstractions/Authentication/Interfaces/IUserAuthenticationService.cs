using Microsoft.AspNetCore.Identity;
using Overoom.Application.Abstractions.Authentication.DTOs;
using Overoom.Application.Abstractions.Authentication.Entities;

namespace Overoom.Application.Abstractions.Authentication.Interfaces;

public interface IUserAuthenticationService
{
    Task CreateAsync(UserCreateDto userDto, string confirmUrl);
    Task<UserData> PasswordAuthenticateAsync(string email, string password);
    Task ResetPasswordAsync(string email, string code, string newPassword);
    Task RequestResetPasswordAsync(string email, string resetUrl);
    Task<UserData> CodeAuthenticateAsync(string userId, string code);
    Task<UserData> ExternalAuthenticateAsync(ExternalLoginInfo info);
}