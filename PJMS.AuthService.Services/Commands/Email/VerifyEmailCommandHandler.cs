using MediatR;
using Microsoft.AspNetCore.Identity;
using PJMS.AuthService.Abstractions.Commands.Email;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Exceptions;

namespace PJMS.AuthService.Services.Commands.Email;

/// <summary>
/// Обработчик команды подтверждения электронной почты пользователя.
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
public class VerifyEmailCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<VerifyEmailCommand>
{
    /// <summary>
    /// Метод обработки команды подтверждения электронной почты пользователя.
    /// </summary>
    /// <param name="request">Запрос подтверждения электронной почты.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <exception cref="UserNotFoundException">Вызывается, если пользователь не найден.</exception>
    /// <exception cref="InvalidCodeException">Вызывается, если код подтверждения недействителен.</exception>
    public async Task Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        // Поиск пользователя по идентификатору; если не найден, вызываем исключение UserNotFoundException.
        var user = await userManager.FindByIdAsync(request.UserId.ToString()) ?? throw new UserNotFoundException();
        
        // Попытка подтверждения электронной почты.
        var result = await userManager.ConfirmEmailAsync(user, request.Code);
        
        // Проверка успешности подтверждения; если не удалось, вызываем исключение InvalidCodeException.
        if (!result.Succeeded) throw new InvalidCodeException();
    }
}