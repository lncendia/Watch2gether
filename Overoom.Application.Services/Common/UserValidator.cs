using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Overoom.Application.Abstractions.Authentication.Entities;

namespace Overoom.Application.Services.Common;

public partial class UserValidator : IUserValidator<UserData>
{
    public async Task<IdentityResult> ValidateAsync(UserManager<UserData> manager, UserData user)
    {
        var errors = new List<IdentityError>();

        ValidateUserName(user, errors);
        await ValidateEmailAsync(user, manager, errors);

        return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
    }

    private static void ValidateUserName(UserData user, ICollection<IdentityError> errors)
    {
        if (string.IsNullOrEmpty(user.UserName) || user.UserName.Length is < 3 or > 20)
        {
            errors.Add(new IdentityError { Description = "Имя не может быть пустым.", Code = "NameLength" });
        }
        else if (!MyRegex().IsMatch(user.UserName))
        {
            errors.Add(new IdentityError
            {
                Description =
                    $"Имя пользователя {user.UserName} недопустимо, имя может содержать только буквы или цифры.",
                Code = "NameFormat"
            });
        }
    }
    
    private static async Task ValidateEmailAsync(UserData user, UserManager<UserData> manager,
        ICollection<IdentityError> errors)
    {
        var email = user.Email;

        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add(new IdentityError { Description = "Почта не может быть пустой.", Code = "MailFormat" });
            return;
        }

        try
        {
            _ = new MailAddress(email);
        }
        catch (FormatException)
        {
            errors.Add(new IdentityError { Description = "Формат почты некорректный.", Code = "MailFormat" });
            return;
        }

        var owner = await manager.FindByEmailAsync(email);
        if (owner != null && !owner.Id.Equals(user.Id))
        {
            errors.Add(new IdentityError { Description = $"Почта {user.Email} уже используется.", Code = "MailUsed" });
        }
    }

    [GeneratedRegex("^[a-zA-Zа-яА-Я0-9_ ]+$")]
    private static partial Regex MyRegex();
}