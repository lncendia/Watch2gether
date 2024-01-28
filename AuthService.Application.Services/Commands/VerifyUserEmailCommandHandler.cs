using AuthService.Application.Abstractions.Commands;
using AuthService.Application.Abstractions.Entities;
using AuthService.Application.Abstractions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services.Commands;

/// <summary>
/// Обработчик команды подтверждения электронной почты пользователя.
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
public class VerifyUserEmailCommandHandler(UserManager<UserData> userManager) : IRequestHandler<VerifyUserEmailCommand>
{
    /// <summary>
    /// Метод обработки команды подтверждения электронной почты пользователя.
    /// </summary>
    /// <param name="request">Запрос подтверждения электронной почты.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <exception cref="InvalidCodeException">Вызывается, если пользователь не найден.</exception>
    /// <exception cref="UserNotFoundException">Вызывается, если код подтверждения недействителен.</exception>
    public async Task Handle(VerifyUserEmailCommand request, CancellationToken cancellationToken)
    {
        // Поиск пользователя по идентификатору; если не найден, вызываем исключение UserNotFoundException.
        var user = await userManager.FindByIdAsync(request.Id.ToString()) ?? throw new UserNotFoundException();
        
        // Попытка подтверждения электронной почты.
        var result = await userManager.ConfirmEmailAsync(user, request.Code);
        
        // Проверка успешности подтверждения; если не удалось, вызываем исключение InvalidCodeException.
        if (!result.Succeeded) throw new InvalidCodeException();
    }
}