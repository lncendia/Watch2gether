using Room.Application.Abstractions.Common.Exceptions;
using Room.Application.Abstractions.DTOs.FilmRoom;
using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Films.Enums;
using Room.Domain.Rooms.FilmRoom.Entities;

namespace Room.Application.Services.Mappers;

/// <summary>
/// Статический класс для преобразования комнаты в DTO
/// </summary>
public static class FilmRoomMapper
{
    /// <summary>
    /// Преобразует комнату в DTO
    /// </summary>
    /// <param name="room">Комната</param>
    /// <param name="unitOfWork">Единица работы</param>
    /// <returns>DTO</returns>
    /// <exception cref="FilmNotFoundException">Фильм не найден</exception>
    /// <exception cref="UserNotFoundException">Пользователь не найден</exception>
    public static async Task<FilmRoomDto> MapAsync(FilmRoom room, IUnitOfWork unitOfWork)
    {
        // Получаем фильм
        var film = await unitOfWork.FilmRepository.Value.GetAsync(room.FilmId);

        // Если фильм не найден - вызываем исключение
        if (film == null) throw new FilmNotFoundException();

        // Создаем коллекцию для DTO зрителей
        List<FilmViewerDto> viewers = [];

        // Перебираем всех зрителей в комнате
        foreach (var roomViewer in room.Viewers)
        {
            // Преобразовываем зрителя в DTO и добавляем в коллекцию 
            viewers.Add(await MapAsync(roomViewer, unitOfWork));
        }
        
        // Создаем DTO и возвращаем
        return new FilmRoomDto
        {
            FilmId = film.Id,
            FilmTitle = film.Title,
            FilmDescription = film.Description,
            FilmUrl = film.CdnList.First(cdn => cdn.Name == room.CdnName).Url,
            FilmType = FilmType.Film,
            OwnerId = room.Owner.UserId,
            Code = room.Code,
            Messages = room.Messages,
            Viewers = viewers
        };
    }

    /// <summary>
    /// Преобразует зрителя в DTO
    /// </summary>
    /// <param name="viewer">Зритель</param>
    /// <param name="unitOfWork">Единица работы</param>
    /// <returns>DTO</returns>
    /// <exception cref="UserNotFoundException">Пользователь не найден</exception>
    private static async Task<FilmViewerDto> MapAsync(FilmViewer viewer, IUnitOfWork unitOfWork)
    {
        // Получаем пользователя
        var user = await unitOfWork.UserRepository.Value.GetAsync(viewer.UserId);

        // Если пользователь не найден - вызываем исключение
        if (user == null) throw new UserNotFoundException();

        // Создаем DTO для зрителя и возвращаем
        return new FilmViewerDto
        {
            UserId = user.Id,
            UserName = user.UserName,
            PhotoUrl = user.PhotoUrl,
            Pause = viewer.Pause,
            FullScreen = viewer.FullScreen,
            TimeLine = viewer.TimeLine,
            Allows = user.Allows,
            Online = viewer.Online,
            Season = viewer.Season,
            Series = viewer.Series
        };
    }
}