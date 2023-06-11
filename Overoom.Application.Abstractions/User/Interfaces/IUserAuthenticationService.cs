﻿using Microsoft.AspNetCore.Identity;
using Overoom.Application.Abstractions.User.DTOs;
using Overoom.Application.Abstractions.User.Entities;

namespace Overoom.Application.Abstractions.User.Interfaces;

public interface IUserAuthenticationService
{
    Task CreateAsync(UserCreateDto userDto, string confirmUrl);
    Task<UserData> AuthenticateAsync(string username, string password);
    Task ResetPasswordAsync(string email, string code, string newPassword);
    Task RequestResetPasswordAsync(string email, string resetUrl);
    Task<UserData> CodeAuthenticateAsync(string userId, string code);
    Task<UserData> ExternalAuthenticateAsync(ExternalLoginInfo info);
}