using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Overoom.Application.Abstractions.Authentication.Entities;

namespace Overoom.Application.Services.Common;

public partial class PasswordValidator : IPasswordValidator<UserData>
{
    public Task<IdentityResult> ValidateAsync(UserManager<UserData> manager, UserData user, string? password)
    {
        var errors = new List<IdentityError>();

        if (string.IsNullOrEmpty(password) || password.Length is < 8 or > 30)
        {
            errors.Add(new IdentityError
            {
                Code = "PasswordLength",
                Description = "Пароль должен содержать от 8 до 30 символов"
            });
        }
        else if (!MyRegex().IsMatch(password))
        {
            errors.Add(new IdentityError
            {
                Code = "PasswordFormat",
                Description = "Пароль должен содержать буквы латинского алфавита, цифры и специальные символы"
            });
        }

        return Task.FromResult(errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
    }

    [GeneratedRegex(@"^(?=.*?[A-Za-z])^(?=.*?[0-9])^(?=.*?[^a-zA-Z0-9])[a-zA-Z0-9_\/\*.#]+$")]
    private static partial Regex MyRegex();
}