using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using PJMS.AuthService.Abstractions.Entities;

namespace PJMS.AuthService.Services.Validators;

/// <summary>
/// Класс валидатора для пользователя
/// </summary>
public class CustomUserValidator : IUserValidator<AppUser>
{
    /// <summary>
    /// Проверяет пользователя на наличие ошибок валидации.
    /// </summary>
    /// <param name="userManager">Менеджер пользователей.</param>
    /// <param name="user">Пользователь для валидации.</param>
    /// <returns>Результат валидации.</returns>
    public async Task<IdentityResult> ValidateAsync(UserManager<AppUser> userManager, AppUser user)
    {
        // Создаем коллекцию с ошибками
        var errors = new List<IdentityError>();

        // Вызываем метод ValidateUserName для проверки имени пользователя и добавления ошибок в коллекцию errors.
        ValidateUserName(user, errors);

        // Вызываем асинхронный метод ValidateEmailAsync для проверки электронной почты пользователя и добавления ошибок в коллекцию errors.
        await ValidateEmailAsync(user, userManager, errors);

        // Возвращаем результат валидации. Если в коллекции errors есть ошибки, возвращаем IdentityResult с ошибками, иначе возвращаем IdentityResult.Success.
        return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
    }

    /// <summary>
    /// Проверяет имя пользователя на валидность.
    /// </summary>
    /// <param name="user">Пользователь для проверки.</param>
    /// <param name="errors">Коллекция ошибок.</param>
    private static void ValidateUserName(AppUser user, ICollection<IdentityError> errors)
    {
        // Проверяет, является ли имя пользователя пустым или имеет длину больше 40 символов.
        if (string.IsNullOrWhiteSpace(user.UserName) || user.UserName.Length > 40)
        {
            // Добавляет новую ошибку в коллекцию ошибок с описанием и кодом ошибки.
            errors.Add(new IdentityError
            {
                // Описание
                Description = "The username must be up to 40 characters long.",

                // Код ошибки
                Code = "InvalidUserNameLength"
            });
        }
    }

    /// <summary>
    /// Проверяет электронную почту пользователя на наличие ошибок валидации.
    /// </summary>
    /// <param name="user">Пользователь для валидации.</param>
    /// <param name="manager">Менеджер пользователей.</param>
    /// <param name="errors">Коллекция ошибок валидации.</param>
    private static async Task ValidateEmailAsync(AppUser user, UserManager<AppUser> manager,
        ICollection<IdentityError> errors)
    {
        // Сохраняем email пользователя в переменную
        var email = user.Email;

        // Проверяем, является ли электронная почта пустой или состоящей только из пробелов.
        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add(new IdentityError
            {
                // Описание
                Description = "The mail cannot be empty.",

                // Код ошибки
                Code = "InvalidEmail"
            });
            return;
        }

        try
        {
            // Создаем новый экземпляр MailAddress с указанной электронной почтой.
            _ = new MailAddress(email);
        }
        catch (FormatException)
        {
            // Если электронная почта имеет неверный формат, добавляем ошибку в коллекцию.
            errors.Add(new IdentityError
            {
                // Описание
                Description = "Invalid mail format.",

                // Код ошибки
                Code = "InvalidEmail"
            });
            return;
        }

        // Ищем владельца (пользователя) с указанной электронной почтой.
        var owner = await manager.FindByEmailAsync(email);

        // Если владелец существует и его идентификатор не совпадает с идентификатором текущего пользователя, добавляем ошибку в коллекцию.
        if (owner != null && !owner.Id.Equals(user.Id))
        {
            errors.Add(new IdentityError
            {
                // Описание
                Description = $"The mail {user.Email} is already in use.",

                // Код ошибки
                Code = "DuplicateEmail"
            });
        }
    }
}