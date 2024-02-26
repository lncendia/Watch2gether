using System.Security.Claims;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PJMS.AuthService.Abstractions.AppThumbnailStore;
using PJMS.AuthService.Abstractions.Commands.Profile;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Exceptions;

namespace PJMS.AuthService.Services.Commands.Profile;

/// <summary>
/// Обработчик для смены аватара у пользователя
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
/// <param name="thumbnailStore">Хранилище фотографий.</param>
public class ChangeAvatarCommandHandler(UserManager<AppUser> userManager, IThumbnailStore thumbnailStore)
    : IRequestHandler<ChangeAvatarCommand, AppUser>
{
    /// <summary>
    /// Метод обработки команды изменения аватара пользователя.
    /// </summary>
    /// <param name="request">Запрос на аватара у пользователя.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Возвращает обновленного пользователя.</returns>
    /// <exception cref="UserNotFoundException">Вызывается, если пользователь не найден.</exception>
    public async Task<AppUser> Handle(ChangeAvatarCommand request, CancellationToken cancellationToken)
    {
        // Поиск пользователя по идентификатору
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        // Вызываем исключение UserNotFoundException если не найден пользователь
        if (user == null) throw new UserNotFoundException();

        // Сохраняем старый аватар
        var oldThumbnail = user.Thumbnail;

        // Сохраняем новый аватар локально
        var thumbnail = await thumbnailStore.SaveAsync(request.Thumbnail);

        // Устанавливаем пользователю новый аватар
        user.Thumbnail = thumbnail;

        // Обновляем данные пользователя
        await userManager.UpdateAsync(user);

        // Если до этого был установлен аватар
        if (oldThumbnail != null)
        {
            // Заменяем утверждение об аватаре
            await userManager.ReplaceClaimAsync(user, new Claim(JwtClaimTypes.Picture, oldThumbnail.ToString()),
                new Claim(JwtClaimTypes.Picture, user.Thumbnail.ToString()));

            // Удаляем старый аватар
            await thumbnailStore.DeleteAsync(oldThumbnail);
        }
        else
        {
            // Добавляем утверждение об аватаре
            await userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Picture, user.Thumbnail.ToString()));
        }

        // Возвращаем пользователя 
        return user;
    }
}