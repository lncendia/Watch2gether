using AuthService.Application.Abstractions.Commands;
using AuthService.Application.Abstractions.Entities;
using AuthService.Application.Abstractions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services.Commands;

/// <summary>
/// Обработчик для выполнения команды смены пароля
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
public class ChangePasswordCommandHandler(UserManager<UserData> userManager) : IRequestHandler<ChangePasswordCommand, UserData>
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
    public async Task<UserData> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        // Получаем пользователя по id
        var user = await userManager.FindByIdAsync(request.Id.ToString());

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