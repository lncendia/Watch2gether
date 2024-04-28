using System.Diagnostics;
using System.Net;
using Films.Application.Abstractions.Exceptions;
using Films.Domain.Rooms.BaseRoom.Exceptions;
using Films.Domain.Rooms.FilmRooms.Exceptions;

namespace Films.Start.Middlewares;

/// <summary>
/// Промежуточное ПО для обработки исключений.
/// </summary>
public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    /// <summary>
    /// Выполнение обработки запроса.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    /// <returns>Асинхронная задача.</returns>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            // Вызов следующего промежуточного ПО в конвейере обработки запроса.
            await next(context);
        }
        catch (Exception exception)
        {
            // Сообщение об ошибке
            string? message;

            // Статус код по умолчанию (400)
            var statusCode = (int)HttpStatusCode.BadRequest;

            // Секция в стандарте
            var section = "15.5.1";

            // Формируем текст ошибки в зависимости от типа ошибки
            switch (exception)
            {
                case UserNotFoundException:
                    message = "Пользователь не найден";
                    break;
                case FilmNotFoundException:
                    message = "Фильм не найден";
                    break;
                case ArgumentException:
                    message = "Некорректные данные";
                    break;
                case PosterSaveException:
                    message = "Некорректный формат изображения";
                    break;
                case UriFormatException:
                    message = "Неверный формат ссылки";
                    break;
                case InvalidCodeException:
                    message = "Код неверный";
                    break;
                case NoSuitableServerException:
                    message = "Нет подходящего сервера";
                    break;
                case RoomIsFullException:
                    message = "Комната заполнена";
                    break;
                case UserBannedInRoomException:
                    message = "Вы были забанены в этой комнате";
                    break;
                case CdnNotFoundException:
                    message = "Указанный CDN не найден";
                    break;
                
                default: // Действие по умолчанию

                    // Устанавливаем статус код 500
                    statusCode = (int)HttpStatusCode.InternalServerError;

                    // Устанавливаем секцию на этот статус код в стандарте
                    section = "15.6.1";

                    // Устанавливаем сообщение ошибки
                    message = exception.Message;

                    // Логгируем исключение
                    logger.LogError(exception, "Возникла ошибка при обработке запроса");
                    break;
            }

            // Установка статуса кода ответа.
            context.Response.StatusCode = statusCode;

            // Формирование объекта ошибки для отправки клиенту.
            var errorResponse = new
            {
                // Ошибка
                errors = new { Error = message },

                // Ссылка на стандарт
                type = $"https://tools.ietf.org/html/rfc9110#section-{section}",

                // Заголовок
                title = "One or more errors occurred.",

                // Статус запроса
                status = context.Response.StatusCode,

                // Уникальный идентификатор запроса
                traceId = Activity.Current?.Id ?? context.TraceIdentifier
            };

            // Отправка объекта ошибки в формате JSON.
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}