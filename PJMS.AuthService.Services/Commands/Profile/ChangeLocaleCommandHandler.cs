using System.Security.Claims;
using IdentityModel;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Overoom.IntegrationEvents.Users;
using PJMS.AuthService.Abstractions.Accessories;
using PJMS.AuthService.Abstractions.Commands.Profile;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Exceptions;

namespace PJMS.AuthService.Services.Commands.Profile;

/// <summary>
/// Обработчик для смены локали у пользователя
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
/// <param name="publishEndpoint">Сервис публикации событий</param>
public class ChangeLocaleCommandHandler(UserManager<AppUser> userManager, IPublishEndpoint publishEndpoint) : IRequestHandler<ChangeLocaleCommand>
{
    /// <summary>
    /// Метод обработки команды изменения локали пользователя.
    /// </summary>
    /// <param name="request">Запрос на смену локали у пользователя.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Возвращает обновленного пользователя.</returns>
    /// <exception cref="UserNotFoundException">Вызывается, если пользователь не найден.</exception>
    /// <exception cref="UserNameLengthException">Вызывается, если имя пользователя имеет некорректную длину.</exception>
    public async Task Handle(ChangeLocaleCommand request, CancellationToken cancellationToken)
    {
        // Поиск пользователя по идентификатору
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        // Вызываем исключение UserNotFoundException если не найден пользователь
        if (user == null) throw new UserNotFoundException();

        // Запоминаем старую локаль
        var oldLocale = user.Locale;

        // Устанавливаем локаль
        user.Locale = request.Localization;

        // Обновляем данные пользователя
        await userManager.UpdateAsync(user);

        // Заменяем локаль в утверждениях пользователя
        await userManager.ReplaceClaimAsync(user, new Claim(JwtClaimTypes.Locale, oldLocale.GetLocalizationString()),
            new Claim(JwtClaimTypes.Locale, user.Locale.GetLocalizationString()));
        
        // Публикуем событие
        await publishEndpoint.Publish(new UserDataChangedIntegrationEvent
        {
            Id = user.Id,
            PhotoUrl = user.Thumbnail,
            Name = user.UserName!,
            Email = user.Email!,
            Locale = user.Locale.ToString()
        }, cancellationToken);
    }
}