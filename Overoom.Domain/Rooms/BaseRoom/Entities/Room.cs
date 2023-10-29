using Overoom.Domain.Abstractions;
using Overoom.Domain.Rooms.BaseRoom.Exceptions;
using Overoom.Domain.Rooms.BaseRoom.ValueObjects;

namespace Overoom.Domain.Rooms.BaseRoom.Entities;

/// <summary> 
/// Абстрактный класс, представляющий базовую комнату. 
/// </summary> 
public abstract class Room : AggregateRoot
{
    /// <summary> 
    /// Инициализирует новый экземпляр класса <see cref="Room"/>. 
    /// </summary> 
    /// <param name="isOpen">Флаг открытости комнаты.</param> 
    /// <param name="owner">Владелец комнаты.</param> 
    protected Room(bool isOpen, Viewer owner)
    {
        AddViewer(owner);
        IsOpen = isOpen;
        Owner = owner;
    }

    /// <summary> 
    /// Возвращает флаг открытости комнаты. 
    /// </summary> 
    public bool IsOpen { get; }
    
    /// <summary> 
    /// Возвращает время последней активности комнаты. 
    /// </summary> 
    public DateTime LastActivity { get; private set; } = DateTime.UtcNow;

    /// <summary> 
    /// Список зрителей в комнате. 
    /// </summary> 
    private readonly List<Viewer> _viewersList = new();
    
    /// <summary> 
    /// Список сообщений в комнате. 
    /// </summary> 
    private readonly List<Message> _messagesList = new();
    
    /// <summary> 
    /// Владелец комнаты. 
    /// </summary> 
    protected readonly Viewer Owner;
    
    /// <summary> 
    /// Счетчик идентификаторов зрителей. 
    /// </summary> 
    private int _idCounter = 1;

    /// <summary> 
    /// Возвращает список сообщений в комнате. 
    /// </summary> 
    public IReadOnlyCollection<Message> Messages => _messagesList.AsReadOnly();
    
    /// <summary> 
    /// Возвращает список зрителей в комнате. 
    /// </summary> 
    protected IReadOnlyCollection<Viewer> Viewers => _viewersList.AsReadOnly();

    /// <summary> 
    /// Устанавливает флаг полноэкранного режима для заданного зрителя. 
    /// </summary> 
    /// <param name="viewerId">Идентификатор зрителя.</param> 
    /// <param name="isFullScreen">Флаг полноэкранного режима.</param> 
    public void SetFullScreen(int viewerId, bool isFullScreen)
    {
        // Получает объект зрителя по его идентификатору. 
        var viewer = GetViewer(viewerId);
        
        // Устанавливает флаг полноэкранного режима для зрителя. 
        viewer.FullScreen = isFullScreen;
        
        // Обновляет время последней активности комнаты.
        UpdateActivity();
    }

    /// <summary> 
    /// Устанавливает флаг онлайн для заданного зрителя. 
    /// </summary> 
    /// <param name="viewerId">Идентификатор зрителя.</param> 
    /// <param name="isOnline">Флаг онлайн.</param> 
    public void SetOnline(int viewerId, bool isOnline)
    {
        // Получает объект зрителя по его идентификатору. 
        var viewer = GetViewer(viewerId);
        
        // Устанавливает флаг онлайн для зрителя. 
        viewer.Online = isOnline;
        
        // Если зритель не онлайн
        if (!isOnline)
        {
            // Устанавливает флаг паузы и сбрасывает флаг полноэкранного режима. 
            viewer.Pause = true;
            viewer.FullScreen = false;
        }

        // Обновляет время последней активности комнаты. 
        UpdateActivity();
    }

    /// <summary> 
    /// Устанавливает флаг паузы для заданного зрителя. 
    /// </summary> 
    /// <param name="viewerId">Идентификатор зрителя.</param> 
    /// <param name="pause">Флаг паузы.</param>
    public void SetPause(int viewerId, bool pause)
    {
        // Получает объект зрителя по его идентификатору. 
        var viewer = GetViewer(viewerId);
        
        // Устанавливает флаг паузы для зрителя. 
        viewer.Pause = pause;
        
        // Обновляет время последней активности комнаты. 
        UpdateActivity();
    }

    /// <summary> 
    /// Устанавливает текущую позицию воспроизведения для заданного зрителя. 
    /// </summary> 
    /// <param name="viewerId">Идентификатор зрителя.</param> 
    /// <param name="time">Текущая позиция воспроизведения.</param> 
    public void SetTimeLine(int viewerId, TimeSpan time)
    {
        // Получает объект зрителя по его идентификатору. 
        var viewer = GetViewer(viewerId);
        
        // Устанавливает текущую позицию воспроизведения для зрителя.
        viewer.TimeLine = time;
        
        // Обновляет время последней активности комнаты. 
        UpdateActivity();
    }

    /// <summary> 
    /// Отправляет сообщение от заданного зрителя. 
    /// </summary> 
    /// <param name="viewerId">Идентификатор зрителя.</param> 
    /// <param name="message">Текст сообщения.</param> 
    public void SendMessage(int viewerId, string message)
    {
        
        // Если количество сообщений достигло максимального значения, удаляем самое старое сообщение. 
        if (_messagesList.Count >= 100) _messagesList.RemoveAt(0);
        
        // Создаем новый объект сообщения. 
        var messageV = new Message(viewerId, message);
        
        // Добавляем сообщение в список сообщений. 
        _messagesList.Add(messageV);
        
        // Обновляет время последней активности комнаты. 
        UpdateActivity();
    }

    /// <summary> 
    /// Производит звуковой сигнал от заданного зрителя к целевому зрителю. 
    /// </summary> 
    /// <param name="viewerId">Идентификатор зрителя.</param> 
    /// <param name="target">Идентификатор целевого зрителя.</param> 
    public void Beep(int viewerId, int target)
    {
        // Обновляет время последней активности комнаты. 
        UpdateActivity();
        
        // Проверяем, что идентификатор зрителя и целевого зрителя не совпадают. 
        if (viewerId == target) throw new ActionNotAllowedException();
        
        // Получаем объект целевого зрителя.
        var viewer = GetViewer(target);
        
        // Проверяем, что у целевого зрителя разрешен звуковой сигнал. 
        if (!viewer.Allows.Beep) throw new ActionNotAllowedException();
    }

    /// <summary> 
    /// Производит крик от заданного зрителя к целевому зрителю. 
    /// </summary> 
    /// <param name="viewerId">Идентификатор зрителя.</param> 
    /// <param name="target">Идентификатор целевого зрителя.</param> 
    public void Scream(int viewerId, int target)
    {
        // Обновляет время последней активности комнаты. 
        UpdateActivity();
        
        // Проверяем, что идентификатор зрителя и целевого зрителя не совпадают. 
        if (viewerId == target) throw new ActionNotAllowedException();
        
        // Получаем объект целевого зрителя.
        var viewer = GetViewer(target);
        
        // Проверяем, что у целевого зрителя разрешен крик. 
        if (!viewer.Allows.Scream) throw new ActionNotAllowedException();
    }

    /// <summary> 
    /// Исключает заданного зрителя из комнаты. 
    /// </summary> 
    /// <param name="viewerId">Идентификатор зрителя.</param> 
    /// <param name="target">Идентификатор целевого зрителя.</param> 
    public void Kick(int viewerId, int target)
    {
        // Обновляет время последней активности комнаты. 
        UpdateActivity();
        
        // Проверяем, что идентификатор зрителя и целевого зрителя не совпадают, и идентификатор зрителя равен идентификатору владельца комнаты. 
        if (viewerId == target || viewerId != Owner.Id) throw new ActionNotAllowedException();
        
        // Получаем объект целевого зрителя.
        var targetViewer = GetViewer(target);
        
        // Удаляем целевого зрителя из списка зрителей комнаты.
        _viewersList.Remove(targetViewer);
        
        // Удаляем все сообщения, связанные с целевым зрителем.
        _messagesList.RemoveAll(x => x.ViewerId == target);
    }

    /// <summary> 
    /// Изменяет имя заданного зрителя. 
    /// </summary> 
    /// <param name="viewerId">Идентификатор зрителя.</param> 
    /// <param name="target">Идентификатор целевого зрителя.</param> 
    /// <param name="name">Новое имя.</param> 
    public void ChangeName(int viewerId, int target, string name)
    {
        // Обновляет время последней активности комнаты. 
        UpdateActivity();
        
        // Проверяем, что идентификатор зрителя и целевого зрителя не совпадают. 
        if (viewerId == target) throw new ActionNotAllowedException();
        
        // Получаем объект целевого зрителя.
        var viewer = GetViewer(target);
        
        // Проверяем, что у целевого зрителя разрешено изменение имени. 
        if (!viewer.Allows.Change) throw new ActionNotAllowedException();
        
        // Изменяем имя целевого зрителя. 
        viewer.Name = name;
    }

    /// <summary> 
    /// Обновляет время последней активности комнаты. 
    /// </summary> 
    protected void UpdateActivity() => LastActivity = DateTime.UtcNow;

    /// <summary> 
    /// Возвращает зрителя по его идентификатору. 
    /// </summary> 
    /// <param name="viewerId">Идентификатор зрителя.</param> 
    /// <returns>Объект зрителя.</returns> 
    protected Viewer GetViewer(int viewerId)
    {
        
        // Ищем зрителя в списке зрителей по его идентификатору. 
        var viewer = _viewersList.FirstOrDefault(x => x.Id == viewerId);
        
        // Если зритель не найден, выбрасываем исключение ViewerNotFoundException. 
        if (viewer == null) throw new ViewerNotFoundException();
        
        // Возвращаем найденного зрителя. 
        return viewer;
    }

    /// <summary> 
    /// Возвращает следующий идентификатор для зрителя. 
    /// </summary> 
    /// <returns>Следующий идентификатор.</returns> 
    protected int GetNextId() => _idCounter;

    /// <summary> 
    /// Добавляет зрителя в комнату. 
    /// </summary> 
    /// <param name="viewer">Объект зрителя.</param> 
    /// <returns>Идентификатор добавленного зрителя.</returns> 
    protected int AddViewer(Viewer viewer)
    {
        // Проверяем, что зритель с таким идентификатором уже не существует в списке зрителей. 
        if (_viewersList.Any(x => x.Id == viewer.Id)) throw new ViewerAlreadyExistsException();
        
        // Устанавливаем флаг онлайн для зрителя. 
        viewer.Online = true;
        
        // Добавляем зрителя в список зрителей комнаты. 
        _viewersList.Add(viewer);
        
        // Обновляем время последней активности комнаты.
        UpdateActivity();
        
        // Увеличиваем значение счетчика идентификаторов зрителей. 
        _idCounter++;
        
        // Возвращаем идентификатор добавленного зрителя. 
        return viewer.Id;
    }
}