using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Room.Infrastructure.Storage.Models.BaseRoom;

[PrimaryKey(nameof(Id), nameof(RoomId))]
public abstract class ViewerModel<TR> where TR : RoomModel
{
    public Guid Id { get; set; }

    public Guid RoomId { get; set; }
    public TR Room { get; set; } = null!;

    public bool Online { get; set; }
    public bool FullScreen { get; set; }
    public bool Pause { get; set; }
    public bool Owner { get; set; }
    public TimeSpan TimeLine { get; set; }
    [MaxLength(40)] public string Nickname { get; set; } = null!;

    public Uri PhotoUrl { get; set; } = null!;

    /// <summary>
    /// Разрешение на совершение звукового сигнала.
    /// </summary>
    public bool Beep { get; set; }

    /// <summary>
    /// Разрешение кричать.
    /// </summary>
    public bool Scream { get; set; }

    /// <summary>
    /// Разрешение на изменение.
    /// </summary>
    public bool Change { get; set; }
}