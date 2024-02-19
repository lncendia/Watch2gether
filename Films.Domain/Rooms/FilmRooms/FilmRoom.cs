using System;
using System.Collections.Generic;
using System.Linq;
using Films.Domain.Extensions;
using Films.Domain.Films;
using Films.Domain.Rooms.BaseRoom;
using Films.Domain.Rooms.FilmRooms.Events;
using Films.Domain.Rooms.FilmRooms.Exceptions;
using Films.Domain.Servers;
using Films.Domain.Users;

namespace Films.Domain.Rooms.FilmRooms;

/// <summary> 
/// Класс, представляющий комнату с фильмом. 
/// </summary> 
public class FilmRoom : Room
{
    public FilmRoom(User user, Film film, IEnumerable<Server> servers, bool isOpen, string cdnName) : base(user,
        servers, isOpen)
    {
        if (film.CdnList.All(c => c.Name != cdnName.GetUpper())) throw new CdnNotFoundException();
        CdnName = cdnName;
        FilmId = film.Id;
        AddDomainEvent(new FilmRoomCreatedDomainEvent
        {
            Film = film,
            Owner = user,
            Room = this
        });
    }

    /// <summary> 
    /// Идентификатор фильма.
    /// </summary> 
    public Guid FilmId { get; }

    /// <summary>
    /// Имя CDN.
    /// </summary>
    public string CdnName { get; }
}