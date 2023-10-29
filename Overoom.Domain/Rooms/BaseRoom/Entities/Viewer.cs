using System.Text.RegularExpressions;
using Overoom.Domain.Abstractions;
using Overoom.Domain.Rooms.BaseRoom.DTOs;
using Overoom.Domain.Rooms.BaseRoom.Exceptions;
using Overoom.Domain.Rooms.BaseRoom.ValueObjects;

namespace Overoom.Domain.Rooms.BaseRoom.Entities;


/// <summary> 
/// Абстрактный класс, представляющий зрителя комнаты. 
/// </summary> 
public abstract partial class Viewer : Entity //todo:do private
{
    /// <summary> 
    /// Инициализирует новый экземпляр класса Viewer. 
    /// </summary> 
    /// <param name="id">Идентификатор зрителя.</param> 
    /// <param name="viewer">DTO объект зрителя.</param> 
    protected Viewer(int id, ViewerDto viewer) : base(id)
    {
        Name = viewer.Name;
        AvatarUri = viewer.AvatarUri;
        Allows = new Allows(viewer.Beep, viewer.Scream, viewer.Change);
    }

    private string _name = null!;

    /// <summary> 
    /// Получает или устанавливает имя зрителя. 
    /// </summary> 
    public string Name
    {
        get => _name;
        internal set
        {
            if (MyRegex().IsMatch(value)) _name = value;
            else throw new ViewerInvalidNicknameException();
        }
    }

    /// <summary> 
    /// Получает URI аватара зрителя. 
    /// </summary> 
    public Uri AvatarUri { get; }
    
    /// <summary> 
    /// Получает или устанавливает флаг онлайн для зрителя. 
    /// </summary> 
    public bool Online { get; set; }
    
    /// <summary> 
    /// Получает или устанавливает флаг полноэкранного режима для зрителя. 
    /// </summary> 
    public bool FullScreen { get; set; }
    
    /// <summary> 
    /// Получает или устанавливает флаг паузы для зрителя. 
    /// </summary> 
    public bool Pause { get; set; } = true;
    
    /// <summary> 
    /// Получает или устанавливает текущую позицию воспроизведения для зрителя. 
    /// </summary> 
    public TimeSpan TimeLine { get; set; } = TimeSpan.Zero;
    
    /// <summary> 
    /// Получает объект, содержащий разрешения для зрителя. 
    /// </summary> 
    public Allows Allows { get; }
    
    /// <summary> 
    /// Получает или устанавливает регулярное выражение для проверки имени зрителя. 
    /// </summary> 
    [GeneratedRegex("^[a-zA-Zа-яА-Я0-9_ ]{3,20}$")]
    private static partial Regex MyRegex();
}