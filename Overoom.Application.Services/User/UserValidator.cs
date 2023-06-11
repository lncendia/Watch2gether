using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Overoom.Application.Abstractions.User.Entities;

namespace Overoom.Application.Services.User;

public class UserValidator : IUserValidator<UserData>
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
        if (string.IsNullOrWhiteSpace(user.UserName))
        {
            errors.Add(new IdentityError { Description = "Имя не может быть пустым.", Code = "NameIncorrect" });
        }
        else if (!Regex.IsMatch(user.UserName, @"^[a-zA-Zа-яА-Я0-9_ ]{3,20}$"))
        {
            errors.Add(new IdentityError
            {
                Description =
                    $"Имя пользователя {user.UserName} недопустимо, имя может содержать только буквы или цифры.",
                Code = "NameIncorrect"
            });
        }
    }

// make sure email is not empty, valid, and unique
    private static async Task ValidateEmailAsync(UserData user, UserManager<UserData> manager,
        ICollection<IdentityError> errors)
    {
        var email = user.Email;

        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add(new IdentityError { Description = "Почта не может быть пустой.", Code = "MailIncorrect" });
            return;
        }

        try
        {
            var unused = new MailAddress(email);
        }
        catch (FormatException)
        {
            errors.Add(new IdentityError { Description = "Формат почты некорректный.", Code = "MailIncorrect" });
            return;
        }

        var owner = await manager.FindByEmailAsync(email);
        if (owner != null && !owner.Id.Equals(user.Id))
        {
            errors.Add(new IdentityError { Description = $"Почта {user.Email} уже используется.", Code = "MailUsed" });
        }
    }
}