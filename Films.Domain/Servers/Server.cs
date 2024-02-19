using System;
using Films.Domain.Abstractions;

namespace Films.Domain.Servers;

/// <summary>
/// Класс, представляющий сервер.
/// </summary>
public class Server : AggregateRoot
{
    /// <summary>
    /// Максимальное количество комнат, доступное на сервере.
    /// </summary>
    public required int MaxRoomsCount { get; set; }

    /// <summary>
    /// Текущее количество комнат на сервере.
    /// </summary>
    public int RoomsCount { get; set; }

    /// <summary>
    /// Флаг, указывающий, включен ли сервер.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// URL-адрес сервера.
    /// </summary>
    public required Uri Url { get; set; }
}