using AuthService.Application.Abstractions.Commands;
using AuthService.Application.Abstractions.Entities;
using AuthService.Application.Abstractions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services.Commands;

/// <summary>
/// Обработчик команды удаления внешнего логина пользователя.
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
public class RemoveUserExternalLoginCommandHandler(UserManager<UserData> userManager)
    : IRequestHandler<RemoveUserExternalLoginCommand, UserData>
{
    /// <summary>
    /// Метод обработки команды удаления внешнего логина пользователя.
    /// </summary>
    /// <param name="request">Команда удаления внешнего логина.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Возвращает обновленного пользователя.</returns>
    /// <exception cref="LoginNotFoundException">Вызывается, если пользователь не найден.</exception>
    /// <exception cref="UserNotFoundException">Вызывается, если внешний логин не найден.</exception>
    public async Task<UserData> Handle(RemoveUserExternalLoginCommand request, CancellationToken cancellationToken)
    {
        // Поиск пользователя по идентификатору.
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        
        // Вызываем исключение UserNotFoundException.
        if (user == null) throw new UserNotFoundException();
        
        // Получение всех внешних логинов пользователя.
        var logins = await userManager.GetLoginsAsync(user);
        
        // Поиск внешнего логина по указанному провайдеру
        var login = logins.FirstOrDefault(info => info.LoginProvider == request.Provider);
        
        // Если не найден, вызываем исключение LoginNotFoundException.
        if (login == null) throw new LoginNotFoundException();
        
        // Удаление внешнего логина с использованием UserManager.
        await userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
        
        // Возвращаем пользователя
        return user;
    }
}