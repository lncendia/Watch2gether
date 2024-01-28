using AuthService.Application.Abstractions.Abstractions.AppEmailService;
using AuthService.Application.Abstractions.Abstractions.AppEmailService.Structs;
using AuthService.Application.Abstractions.Commands;
using AuthService.Application.Abstractions.Entities;
using AuthService.Application.Abstractions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services.Commands;

/// <summary>
/// Обработчик команды запроса восстановления пароля пользователя.
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
/// <param name="emailService">Сервис электронной почты для отправки уведомлений.</param>
public class RequestRecoverPasswordCommandHandler(UserManager<UserData> userManager, IEmailService emailService)
    : IRequestHandler<RequestRecoverPasswordCommand>
{
    /// <summary>
    /// Метод обработки команды запроса восстановления пароля пользователя.
    /// </summary>
    /// <param name="request">Запрос на восстановление пароля.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <exception cref="UserNotFoundException">Вызывается, если пользователь не найден.</exception>
    /// <exception cref="EmailNotConfirmedException">Вызывается, если почта пользователя не подтверждена.</exception>
    public async Task Handle(RequestRecoverPasswordCommand request, CancellationToken cancellationToken)
    {
        // Поиск пользователя по адресу электронной почты.
        var user = await userManager.FindByEmailAsync(request.Email);
        
        // Вызываем исключение если пользователь не найден
        if (user == null) throw new UserNotFoundException();

        // Проверка, что почта подтверждена
        if (!await userManager.IsEmailConfirmedAsync(user)) throw new EmailNotConfirmedException();
        
        // Генерация кода сброса пароля.
        var code = await userManager.GeneratePasswordResetTokenAsync(user);
        
        // Формирование URL для подтверждения сброса пароля.
        var url = GenerateMailUrl(request.ResetUrl, user.Id, code);
        
        // Отправка электронного письма с ссылкой для подтверждения сброса пароля.
        await emailService.SendAsync(new ConfirmRecoverPasswordEmail { Recipient = request.Email, ConfirmLink = url });
    }

    /// <summary>
    /// Генерирует URL для подтверждения регистрации по электронной почте.
    /// </summary>
    /// <param name="url">Базовый URL.</param>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="code">Код подтверждения.</param>
    /// <returns>Сгенерированный URL.</returns>
    private static string GenerateMailUrl(string url, long id, string code)
    {
        // Создаем объект UriBuilder с базовым URL
        var uriBuilder = new UriBuilder(url);

        // Получаем коллекцию параметров запроса
        var queryParameters = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);

        // Добавляем параметр "id" со значением
        queryParameters["id"] = id.ToString();

        // Добавляем параметр "code" со значением
        queryParameters["code"] = code;

        // Устанавливаем обновленную строку запроса
        uriBuilder.Query = queryParameters.ToString();

        // Получаем обновленный URL
        return uriBuilder.ToString();
    }
}