using Overoom.Domain.Rooms.BaseRoom.DTOs;
using Overoom.Domain.Rooms.BaseRoom.Entities;
using Overoom.Domain.Rooms.BaseRoom.Exceptions;
using Overoom.Domain.Rooms.YoutubeRoom.Exceptions;

namespace Overoom.Domain.Rooms.YoutubeRoom.Entities;

/// <summary> 
/// Представляет комнату для просмотра видео на YouTube. 
/// </summary> 
public class YoutubeRoom : Room
{
    /// <summary> 
    /// Инициализирует новый экземпляр класса <see cref="YoutubeRoom"/>. 
    /// </summary> 
    /// <param name="url">URL видео.</param> 
    /// <param name="access">Флаг доступа к очереди видео.</param> 
    /// <param name="isOpen">Флаг открытости комнаты.</param> 
    /// <param name="viewer">Информация о зрителе.</param> 
    public YoutubeRoom(Uri url, bool access, bool isOpen, ViewerDto viewer) : base(isOpen,
        CreateViewer(viewer, GetId(url)))
    {
        Access = access;
        _ids.Add(GetId(url));
    }

    /// <summary> 
    /// Создает объект Viewer для заданного зрителя и идентификатора. 
    /// </summary> 
    /// <param name="viewer">Информация о зрителе.</param> 
    /// <param name="id">Идентификатор.</param> 
    /// <returns>Объект Viewer.</returns> 
    private static Viewer CreateViewer(ViewerDto viewer, string id) => new YoutubeViewer(viewer, 1, id);

    /// <summary> 
    /// Возвращает флаг доступа к изменению видео. 
    /// </summary> 
    public bool Access { get; }
    
    /// <summary> 
    /// Возвращает владельца комнаты. 
    /// </summary> 
    public new YoutubeViewer Owner => (YoutubeViewer)base.Owner;
    
    /// <summary> 
    /// Возвращает список зрителей комнаты. 
    /// </summary> 
    public new IReadOnlyCollection<YoutubeViewer> Viewers => base.Viewers.Cast<YoutubeViewer>().ToList();
   
    /// <summary> 
    /// Возвращает список идентификаторов видео. 
    /// </summary> 
    public IReadOnlyCollection<string> VideoIds => _ids.ToList();
    
    /// <summary> 
    /// Список идентификаторов видео. 
    /// </summary> 
    private readonly List<string> _ids = new();

    
    /// <summary> 
    /// Подключает зрителя к комнате. 
    /// </summary> 
    /// <param name="viewer">Информация о зрителе.</param> 
    /// <returns>Идентификатор зрителя.</returns> 
    public int Connect(ViewerDto viewer)
    {
        // Создаем объект YoutubeViewer с информацией о зрителе, получаем следующий идентификатор и передаем текущий идентификатор видео владельца
        var youtubeViewer = new YoutubeViewer(viewer, GetNextId(), Owner.CurrentVideoId);
        
        // Добавляем зрителя в комнату и возвращаем его идентификатор 
        return AddViewer(youtubeViewer);
    }

    /// <summary> 
    /// Изменяет текущее видео для заданного зрителя. 
    /// </summary> 
    /// <param name="viewerId">Идентификатор зрителя.</param> 
    /// <param name="id">Идентификатор видео.</param> 
    public void ChangeVideo(int viewerId, string id)
    {
        // Проверяем, содержится ли идентификатор видео в списке доступных видео
        if (!VideoIds.Contains(id)) throw new VideoNotFoundException();
        
        // Получаем объект YoutubeViewer по его идентификатору
        var viewer = (YoutubeViewer)GetViewer(viewerId);
        
        // Изменяем текущий идентификатор видео для зрителя 
        viewer.CurrentVideoId = id;
    }

    /// <summary> 
    /// Добавляет видео в комнату. 
    /// </summary> 
    /// <param name="viewerId">Идентификатор зрителя.</param> 
    /// <param name="url">URL видео.</param> 
    /// <returns>Идентификатор видео.</returns> 
    public string AddVideo(int viewerId, Uri url)
    {
        // Проверяем, является ли зритель владельцем комнаты или у него есть доступ к комнате 
        if (viewerId != Owner.Id && !Access) throw new ActionNotAllowedException();
        
        // Получаем идентификатор видео из URL 
        var id = GetId(url);
        
        // Добавляем идентификатор видео в список 
        _ids.Add(id);
        
        // Возвращаем идентификатор видео 
        return id;
    }
    
    /// <summary> 
    /// Возвращает идентификатор видео из заданного URL. 
    /// </summary> 
    /// <param name="uri">URL видео.</param> 
    /// <returns>Идентификатор видео.</returns> 
    private static string GetId(Uri uri)
    {
        string id;
        try
        {
            // Получаем идентификатор видео в зависимости от хоста URL
            id = uri.Host switch
            {
                "www.youtube.com" => uri.Query[3..],
                "youtu.be" => uri.Segments[1],
                _ => string.Empty
            };
        }
        catch
        {
            throw new InvalidVideoUrlException();
        }

        // Проверяем, что идентификатор видео не пустой или null
        if (string.IsNullOrEmpty(id)) throw new InvalidVideoUrlException();
        
        // Возвращаем идентификатор видео
        return id;
    }
}