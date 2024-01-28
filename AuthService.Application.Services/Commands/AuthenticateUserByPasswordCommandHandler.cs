using AuthService.Application.Abstractions.Commands;
using AuthService.Application.Abstractions.Entities;
using AuthService.Application.Abstractions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services.Commands;

/// <summary>
/// Класс обработчика команды аутентификации пользователя по паролю.
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
public class AuthenticateUserByPasswordCommandHandler(UserManager<UserData> userManager) : IRequestHandler<AuthenticateUserByPasswordCommand, UserData>
{
    /// <summary>
    /// Обработка команды аутентификации пользователя по паролю.
    /// </summary>
    /// <param name="request">Запрос на аутентификацию пользователя.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Объект пользователя в случае успешной аутентификации.</returns>
    /// <exception cref="UserNotFoundException">Вызывается, если пользователь не найден.</exception>
    /// <exception cref="UserLockoutException">Вызывается, если пользователь заблокирован.</exception>
    /// <exception cref="InvalidPasswordException">Вызывается, если валидация пароля не прошла.</exception>
    public async Task<UserData> Handle(AuthenticateUserByPasswordCommand request, CancellationToken cancellationToken)
    {
        // Получаем пользователя по его электронной почте.
        var user = await userManager.FindByEmailAsync(request.Email);
        
        // Если пользователь не найден, вызываем исключение UserNotFoundException.
        if (user == null) throw new UserNotFoundException();
        
        // Проверяем заблокирован ли пользователь 
        if (await userManager.IsLockedOutAsync(user))
        {
            // Если пользователь заблокирован, вызываем исключение UserLockoutException.
            throw new UserLockoutException();
        }
        
        // Проверяем правильность введенного пароля.
        var success = await userManager.CheckPasswordAsync(user, request.Password);

        // Если пароль верный
        if (success)
        {
            // Cбрасываем счетчик неудачных попыток входа
            await userManager.ResetAccessFailedCountAsync(user);
            
            // Устанавливаем время последнего входа
            user.TimeLastAuth = DateTime.Now;

            // Обновляем данные
            await userManager.UpdateAsync(user);
            
            //Возвращаем пользователя
            return user;
        }
        
        // Если пароль неверный
        // Инкрементируем счетчик неудачных попыток
        await userManager.AccessFailedAsync(user);
        
        // Вызываем исключение InvalidPasswordException.
        throw new InvalidPasswordException();
    }
}