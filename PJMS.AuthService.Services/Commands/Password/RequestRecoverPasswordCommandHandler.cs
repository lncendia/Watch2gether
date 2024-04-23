using MediatR;
using Microsoft.AspNetCore.Identity;
using PJMS.AuthService.Abstractions.AppEmailService;
using PJMS.AuthService.Abstractions.AppEmailService.Structs;
using PJMS.AuthService.Abstractions.Commands.Password;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Exceptions;

namespace PJMS.AuthService.Services.Commands.Password;

/// <summary>
/// Обработчик команды запроса восстановления пароля пользователя.
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
/// <param name="emailService">Сервис электронной почты для отправки уведомлений.</param>
public class RequestRecoverPasswordCommandHandler(UserManager<AppUser> userManager, IEmailService emailService)
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
        var url = GenerateMailUrl(request.ResetUrl, user.Email!, code);

        // Отправка электронного письма с ссылкой для подтверждения сброса пароля.
        await emailService.SendAsync(new ConfirmRecoverPasswordEmail { Recipient = request.Email, ConfirmLink = url });
    }

    /// <summary>
    /// Генерирует URL для подтверждения регистрации по электронной почте.
    /// </summary>
    /// <param name="url">Базовый URL.</param>
    /// <param name="email">Почта пользователя.</param>
    /// <param name="code">Код подтверждения.</param>
    /// <returns>Сгенерированный URL.</returns>
    private static string GenerateMailUrl(string url, string email, string code)
    {
        // Создаем объект UriBuilder с базовым URL
        var uriBuilder = new UriBuilder(url);

        // Получаем коллекцию параметров запроса
        var queryParameters = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);

        // Добавляем параметр "email" со значением
        queryParameters["email"] = email;

        // Добавляем параметр "code" со значением
        queryParameters["code"] = code;

        // Устанавливаем обновленную строку запроса
        uriBuilder.Query = queryParameters.ToString();

        // Получаем обновленный URL
        return uriBuilder.ToString();
    }
}