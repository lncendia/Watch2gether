using System.Net.Mail;
using System.Text.RegularExpressions;
using AuthService.Application.Abstractions.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services.Validators;

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
            errors.Add(new IdentityError
            {
                Description = "The user name must be between 3 and 20 characters long.", Code = "InvalidUserNameLength"
            });
        }
        else if (!MyRegex().IsMatch(user.UserName))
        {
            errors.Add(new IdentityError
            {
                Description =
                    $"The user name {user.UserName} is invalid, the name can contain only letters or numbers",
                Code = "InvalidUserName"
            });
        }
    }

    private static async Task ValidateEmailAsync(UserData user, UserManager<UserData> manager,
        ICollection<IdentityError> errors)
    {
        var email = user.Email;

        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add(new IdentityError { Description = "The mail cannot be empty.", Code = "InvalidEmail" });
            return;
        }

        try
        {
            _ = new MailAddress(email);
        }
        catch (FormatException)
        {
            errors.Add(new IdentityError { Description = "The mail format is incorrect.", Code = "InvalidEmail" });
            return;
        }

        var owner = await manager.FindByEmailAsync(email);
        if (owner != null && !owner.Id.Equals(user.Id))
        {
            errors.Add(new IdentityError
                { Description = $"The mail {user.Email} is already in use.", Code = "DuplicateEmail" });
        }
    }

    [GeneratedRegex("^[a-zA-Zа-яА-Я0-9_ ]+$")]
    private static partial Regex MyRegex();
}