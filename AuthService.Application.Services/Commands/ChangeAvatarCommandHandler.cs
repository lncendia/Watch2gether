using System.Security.Claims;
using AuthService.Application.Abstractions.Abstractions.AppThumbnailStore;
using AuthService.Application.Abstractions.Commands;
using AuthService.Application.Abstractions.Entities;
using AuthService.Application.Abstractions.Exceptions;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services.Commands;

/// <summary>
/// Обработчик для смены аватара у пользователя
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
/// <param name="thumbnailStore">Хранилище фотографий.</param>
public class ChangeAvatarCommandHandler(UserManager<UserData> userManager, IThumbnailStore thumbnailStore)
    : IRequestHandler<ChangeAvatarCommand, UserData>
{
    /// <summary>
    /// Метод обработки команды изменения аватара пользователя.
    /// </summary>
    /// <param name="request">Запрос на аватара у пользователя.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Возвращает обновленного пользователя.</returns>
    /// <exception cref="UserNotFoundException">Вызывается, если пользователь не найден.</exception>
    /// <exception cref="UserNameFormatException">Вызывается, если имя пользователя имеет некорректный формат.</exception>
    /// <exception cref="UserNameLengthException">Вызывается, если имя пользователя имеет некорректную длину.</exception>
    public async Task<UserData> Handle(ChangeAvatarCommand request, CancellationToken cancellationToken)
    {
        // Поиск пользователя по идентификатору
        var user = await userManager.FindByIdAsync(request.Id.ToString());

        // Вызываем исключение UserNotFoundException если не найден пользователь
        if (user == null) throw new UserNotFoundException();

        // Сохраняем старый аватар
        var oldThumbnail = user.AvatarUrl;

        // Сохраняем новый аватар локально
        var thumbnail = await thumbnailStore.SaveAsync(request.Avatar);

        // Устанавливаем пользователю новый аватар
        user.AvatarUrl = thumbnail;

        // Обновляем данные пользователя
        await userManager.UpdateAsync(user);

        // Заменяем утверждение об аватаре
        await userManager.ReplaceClaimAsync(user, new Claim(JwtClaimTypes.Picture, oldThumbnail.ToString()),
            new Claim(JwtClaimTypes.Picture, user.AvatarUrl.ToString()));

        // Удаляем старый аватар
        await thumbnailStore.DeleteAsync(oldThumbnail);

        // Возвращаем пользователя 
        return user;
    }
}