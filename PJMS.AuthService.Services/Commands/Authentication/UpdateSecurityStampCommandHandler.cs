using MediatR;
using Microsoft.AspNetCore.Identity;
using PJMS.AuthService.Abstractions.Commands.Authentication;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Exceptions;

namespace PJMS.AuthService.Services.Commands.Authentication;

/// <summary>
/// Обработчик команды для закрытия других сессий пользователя.
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
public class UpdateSecurityStampCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<UpdateSecurityStampCommand, AppUser>
{
    /// <summary>
    /// Обработка команды на закрытие других сессий у пользователя.
    /// </summary>
    /// <param name="request">Запрос на закрытие сессий у пользователя.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Возвращает пользователя с обновленным Security Stamp в случае успеха.</returns>
    /// <exception cref="UserNotFoundException">Вызывается, если пользователь не найден.</exception>
    public async Task<AppUser> Handle(UpdateSecurityStampCommand request, CancellationToken cancellationToken)
    {
        // Получаем пользователя по id.
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        // Выкидываем исключение если пользователь не найден.
        if (user == null) throw new UserNotFoundException();

        // Обновляем SecurityStamp, так как данные у пользователя поменялись.
        await userManager.UpdateSecurityStampAsync(user);
        
        // Возвращаем пользователя.
        return user;
    }
}