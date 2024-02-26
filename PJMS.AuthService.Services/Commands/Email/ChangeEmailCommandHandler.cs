using MediatR;
using Microsoft.AspNetCore.Identity;
using PJMS.AuthService.Abstractions.Commands.Email;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Exceptions;

namespace PJMS.AuthService.Services.Commands.Email;

/// <summary>
/// Обработчик для смены почта у пользователя
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
public class ChangeEmailCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<ChangeEmailCommand, AppUser>
{
    /// <summary>
    /// Метод обработки команды изменения электронной почты пользователя.
    /// </summary>
    /// <param name="request">Запрос на смену почты у пользователя.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Возвращает обновленного пользователя.</returns>
    /// <exception cref="UserNotFoundException">Вызывается, если пользователь не найден.</exception>
    public async Task<AppUser> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
    {
        // Поиск пользователя по идентификатору
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        // Вызываем исключение UserNotFoundException если не найден пользователь
        if (user == null) throw new UserNotFoundException();

        // Попытка изменения электронной почты пользователя.
        var result = await userManager.ChangeEmailAsync(user, request.NewEmail, request.Code);

        // Если результат неудачный
        if (!result.Succeeded)
        {
            // Если хоть одна ошибка DuplicateEmail, то вызываем исключение 
            if (result.Errors.Any(error => error.Code == "DuplicateEmail")) throw new EmailAlreadyTakenException();

            // Если хоть одна ошибка InvalidToken, то вызываем исключение 
            if (result.Errors.Any(error => error.Code == "InvalidToken")) throw new InvalidCodeException();

            // Если хоть одна ошибка InvalidEmail, то вызываем исключение 
            if (result.Errors.Any(e => e.Code == "InvalidEmail")) throw new EmailFormatException();
        }

        // Устанавливаем имя
        await userManager.SetUserNameAsync(user, request.NewEmail);

        // Возвращаем пользователя 
        return user;
    }
}