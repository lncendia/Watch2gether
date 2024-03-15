using MediatR;
using Room.Application.Abstractions.Commands.Rooms;
using Room.Domain.Rooms.FilmRooms.ValueObjects;

namespace Room.Application.Abstractions.Commands.FilmRooms;

/// <summary>
/// Команда на создание комнаты
/// </summary>
public class CreateRoomCommand : IRequest
{
    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public required Guid Id { get; init; }
    
    /// <summary>
    /// Зритель
    /// </summary>
    public required ViewerData Owner { get; init; }

    /// <summary> 
    /// Заголовок фильма. 
    /// </summary> 
    public required string Title { get; init; }

    /// <summary> 
    /// Поставщик фильма
    /// </summary> 
    public required Cdn Cdn { get; init; }

    /// <summary> 
    /// Тип фильма
    /// </summary> 
    public required bool IsSerial { get; init; }
}