using Room.Application.Abstractions.Common.Exceptions;
using Room.Application.Abstractions.DTOs.YoutubeRoom;
using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Rooms.YoutubeRoom.Entities;

namespace Room.Application.Services.Mappers;

/// <summary>
/// Статический класс для преобразования комнаты в DTO
/// </summary>
public static class YoutubeRoomMapper
{
    /// <summary>
    /// Преобразует комнату в DTO
    /// </summary>
    /// <param name="room">Комната</param>
    /// <param name="unitOfWork">Единица работы</param>
    /// <returns>DTO</returns>
    /// <exception cref="FilmNotFoundException">Фильм не найден</exception>
    /// <exception cref="UserNotFoundException">Пользователь не найден</exception>
    public static async Task<YoutubeRoomDto> MapAsync(YoutubeRoom room, IUnitOfWork unitOfWork)
    {
        // Создаем коллекцию для DTO зрителей
        List<YoutubeViewerDto> viewers = [];

        // Перебираем всех зрителей в комнате
        foreach (var roomViewer in room.Viewers)
        {
            // Преобразовываем зрителя в DTO и добавляем в коллекцию 
            viewers.Add(await MapAsync(roomViewer, unitOfWork));
        }

        // Создаем DTO и возвращаем
        return new YoutubeRoomDto
        {
            OwnerId = room.Owner.UserId,
            Code = room.Code,
            Messages = room.Messages,
            Viewers = viewers,
            Ids = room.Videos,
            VideoAccess = room.VideoAccess
        };
    }

    /// <summary>
    /// Преобразует зрителя в DTO
    /// </summary>
    /// <param name="viewer">Зритель</param>
    /// <param name="unitOfWork">Единица работы</param>
    /// <returns>DTO</returns>
    /// <exception cref="UserNotFoundException">Пользователь не найден</exception>
    private static async Task<YoutubeViewerDto> MapAsync(YoutubeViewer viewer, IUnitOfWork unitOfWork)
    {
        // Получаем пользователя
        var user = await unitOfWork.UserRepository.Value.GetAsync(viewer.UserId);

        // Если пользователь не найден - вызываем исключение
        if (user == null) throw new UserNotFoundException();

        // Создаем DTO для зрителя и возвращаем
        return new YoutubeViewerDto
        {
            UserId = user.Id,
            UserName = user.UserName,
            PhotoUrl = user.PhotoUrl,
            Pause = viewer.Pause,
            FullScreen = viewer.FullScreen,
            TimeLine = viewer.TimeLine,
            Allows = user.Allows,
            Online = viewer.Online,
            CurrentVideoId = viewer.CurrentVideoNumber
        };
    }
}