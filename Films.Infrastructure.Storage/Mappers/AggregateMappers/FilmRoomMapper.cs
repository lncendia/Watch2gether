using System.Reflection;
using System.Runtime.CompilerServices;
using Films.Domain.Abstractions;
using Films.Domain.Rooms.FilmRooms;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Mappers.StaticMethods;
using Films.Infrastructure.Storage.Models.FilmRooms;

namespace Films.Infrastructure.Storage.Mappers.AggregateMappers;

/// <summary>
/// Класс для отображения модели FilmRoomModel на агрегат FilmRoom.
/// </summary>
internal class FilmRoomMapper : IAggregateMapperUnit<FilmRoom, FilmRoomModel>
{
    /// <summary>
    /// Тип агрегата.
    /// </summary>
    private static readonly Type FilmRoomType = typeof(FilmRoom);

    /// <summary>
    /// Поле для хранения идентификатора фильма в агрегате.
    /// </summary>
    private static readonly FieldInfo FilmId =
        FilmRoomType.GetField("<FilmId>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    /// <summary>
    /// Поле для хранения имени CDN в агрегате.
    /// </summary>
    private static readonly FieldInfo CdnName =
        FilmRoomType.GetField("<CdnName>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    /// <summary>
    /// Отображает модель <see cref="FilmRoomModel"/> на агрегат <see cref="FilmRoom"/>.
    /// </summary>
    /// <param name="model">Модель для отображения.</param>
    /// <returns>Агрегат <see cref="FilmRoom"/>.</returns>
    public Task<FilmRoom> MapAsync(FilmRoomModel model)
    {
        // Создает новый экземпляр агрегата без вызова конструктора.
        var room = (FilmRoom)RuntimeHelpers.GetUninitializedObject(FilmRoomType);
        
        // Устанавливает идентификатор фильма в агрегате.
        FilmId.SetValue(room, model.FilmId);
        
        // Устанавливает имя CDN в агрегате.
        CdnName.SetValue(room, model.CdnName);
        
        // Устанавливает дату создания комнаты в агрегате.
        RoomFields.CreationDate.SetValue(room, model.CreationDate);
        
        // Устанавливает код комнаты в агрегате.
        RoomFields.Code.SetValue(room, model.Code);
        
        // Устанавливает идентификатор сервера в агрегате.
        RoomFields.ServerId.SetValue(room, model.ServerId);
        
        // Устанавливает список идентификаторов зрителей в агрегате.
        RoomFields.Viewers.SetValue(room, model.Viewers.Select(v => v.UserId).ToList());
        
        // Устанавливает список идентификаторов заблокированных пользователей в агрегате.
        RoomFields.Banned.SetValue(room, model.Banned.Select(b => b.UserId).ToList());

        // Устанавливает идентификатор агрегата.
        IdFields.AggregateId.SetValue(room, model.Id);
        
        // Устанавливает список событий домена в агрегате.
        IdFields.DomainEvents.SetValue(room, new List<IDomainEvent>());
        
        // Возвращает экземпляр агрегата.
        return Task.FromResult(room);
    }
}
