using System.Text.RegularExpressions;
using AuthService.Application.Abstractions.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services.Validators;

/// <summary>
/// Класс валидатора для пароля
/// </summary>
public partial class PasswordValidator : IPasswordValidator<UserData>
{
    /// <summary>
    /// Метод валидации пароля
    /// </summary>
    /// <param name="manager">Менеджер пользователей (UserManager)</param>
    /// <param name="user">Пользователь, для которого проводится валидация</param>
    /// <param name="password">Пароль, который требуется проверить</param>
    /// <returns>Возвращает результат валидации в виде объекта IdentityResult</returns>
    public Task<IdentityResult> ValidateAsync(UserManager<UserData> manager, UserData user, string? password)
    {
        // Замена множественных пробелов на одиночный
        password = MyRegex().Replace(password!, " ");

        // Проверка на длину пароля
        if (password.Length is < 8 or > 128)
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Description = "Password length should be from 8 to 128 characters.",
                Code = "InvalidPasswordLength"
            }));
        }
        
        // Проверка на наличие букв в верхнем регистре
        var hasUpperChar = password.Any(char.IsUpper);
        
        // Проверка на наличие букв в нижнем регистре
        var hasLowerChar = password.Any(char.IsLower);
        
        // Проверка на наличие цифр
        var hasDigit = password.Any(char.IsDigit);
        
        // Проверка на наличие специальных символов
        var hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

        // Если все условия соблюдаются, то возвращаем Success
        if (hasUpperChar && hasLowerChar && hasDigit && hasSpecialChar)
        {
            return Task.FromResult(IdentityResult.Success);
        }
        
        // Создаем список IdentityError
        var errors = new List<IdentityError>();

        // Проверка на наличие букв в верхнем регистре
        if (!hasUpperChar)
        {
            errors.Add(new IdentityError { Description = "Password must contain uppercase letters.", Code = "PasswordRequiresUpper"});
        }

        // Проверка на наличие букв в нижнем регистре
        if (!hasLowerChar)
        {
            errors.Add(new IdentityError { Description = "Password must contain lowercase letters.", Code = "PasswordRequiresLower"});
        }

        // Проверка на наличие цифр
        if (!hasDigit)
        {
            errors.Add(new IdentityError { Description = "Password must contain digits.", Code = "PasswordRequiresDigit"});
        }

        // Проверка на наличие специальных символов
        if (!hasSpecialChar)
        {
            errors.Add(new IdentityError { Description = "Password must contain special characters.", Code = "PasswordRequiresNonAlphanumeric"});
        }

        // Возвращаем массив IdentityError
        return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex MyRegex();
}