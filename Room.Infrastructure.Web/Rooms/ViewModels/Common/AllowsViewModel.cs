namespace Room.Infrastructure.Web.Rooms.ViewModels.Common;

public class AllowsViewModel
{
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