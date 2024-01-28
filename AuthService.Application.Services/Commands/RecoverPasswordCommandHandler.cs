using AuthService.Application.Abstractions.Commands;
using AuthService.Application.Abstractions.Entities;
using AuthService.Application.Abstractions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services.Commands;

/// <summary>
/// Обработчик для выполнения команды сброса пароля
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
public class RecoverPasswordCommandHandler(UserManager<UserData> userManager) : IRequestHandler<RecoverPasswordCommand>
{
    /// <summary>
    /// Обработка команды RecoverPasswordCommand для сброса пароля.
    /// </summary>
    /// <param name="request">Запрос на сброс пароля пользователя.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <exception cref="UserNotFoundException">Вызывается, если пользователь не найден.</exception>
    /// <exception cref="InvalidCodeException">Вызывается, если введенный код неверен.</exception>
    /// <exception cref="PasswordValidationException">Вызывается, если валидация пароля не прошла.</exception>
    public async Task Handle(RecoverPasswordCommand request, CancellationToken cancellationToken)
    {
        // Находим пользователя по email
        var user = await userManager.FindByEmailAsync(request.Email);

        // Выкидаем исключение если пользователь не найден
        if (user == null) throw new UserNotFoundException();

        // Сбрасываем пароль, обновляя его на request.NewPassword
        var result = await userManager.ResetPasswordAsync(user, request.Code, request.NewPassword);

        // Если результат неудачный
        if (!result.Succeeded)
        {
            // Если хоть одна ошибка InvalidCode, то вызываем исключение 
            if (result.Errors.Any(e => e.Code == "InvalidCode")) throw new InvalidCodeException();

            //Создаем словарь для хранения ошибок
            var passwordValidationErrors = result.Errors.ToDictionary(e => e.Code, e => e.Description);

            // Вызываем исключение, содержащие в себе словарь ошибок валидации пароля
            throw new PasswordValidationException { ValidationErrors = passwordValidationErrors };
        }
    }
}