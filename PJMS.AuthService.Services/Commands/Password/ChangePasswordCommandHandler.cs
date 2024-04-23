using MediatR;
using Microsoft.AspNetCore.Identity;
using PJMS.AuthService.Abstractions.Commands.Password;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Exceptions;

namespace PJMS.AuthService.Services.Commands.Password;

/// <summary>
/// Обработчик для выполнения команды смены пароля
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
public class ChangePasswordCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<ChangePasswordCommand, AppUser>
{
    /// <summary>
    /// Обработка команды ChangePasswordCommand, обновляя пароль пользователя.
    /// </summary>
    /// <param name="request">Запрос на смену пароля пользователя.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Возвращает обновленного пользователя.</returns>
    /// <exception cref="UserNotFoundException">Вызывается, если пользователь не найден.</exception>
    /// <exception cref="OldPasswordNeededException">Вызывается, если не введен старый пароль.</exception>
    /// <exception cref="PasswordValidationException">Вызывается, если валидация пароля не прошла.</exception>
    public async Task<AppUser> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        // Получаем пользователя по id
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        // Вызываем исключение если пользователь не найден
        if (user == null) throw new UserNotFoundException();

        // Создаем объект result для дальнейшего сохранения в него результата смены пароля
        IdentityResult result;

        // Проверяем есть ли хэш пароля у пользователя
        if (user.PasswordHash == null)
        {
            // Меняем пароль
            result = await userManager.AddPasswordAsync(user, request.NewPassword);
        }
        else
        {
            // Если нет старого пароля, то выкидваем исключение
            if (request.OldPassword == null) throw new OldPasswordNeededException();

            // Меняем пароль
            result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        }


        // Проверка успешности операции смены пароля у пользователя
        if (!result.Succeeded)
        {
            //Создаем словарь для хранения ошибок
            var passwordValidationErrors = result.Errors.ToDictionary(e => e.Code, e => e.Description);

            // Вызываем исключение, содержащие в себе словарь ошибок валидации пароля
            throw new PasswordValidationException { ValidationErrors = passwordValidationErrors };
        }

        // Возвращаем пользователя
        return user;
    }
}