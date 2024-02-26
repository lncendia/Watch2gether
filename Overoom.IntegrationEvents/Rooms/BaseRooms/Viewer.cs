namespace Overoom.IntegrationEvents.Rooms.BaseRooms;

public class Viewer
{
    public required Guid Id { get; init; }
    
    public required Uri PhotoUrl { get; init; }
    
    public required string Name { get; init; }
    
    /// <summary>
    /// Разрешение на совершение звукового сигнала.
    /// </summary>
    public required bool Beep { get; init; }

    /// <summary>
    /// Разрешение кричать.
    /// </summary>
    public required bool Scream { get; init; }

    /// <summary>
    /// Разрешение на изменение.
    /// </summary>
    public required bool Change { get; init; }
}