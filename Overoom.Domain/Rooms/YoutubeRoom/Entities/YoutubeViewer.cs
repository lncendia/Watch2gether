using Overoom.Domain.Rooms.BaseRoom.DTOs;
using Overoom.Domain.Rooms.BaseRoom.Entities;

namespace Overoom.Domain.Rooms.YoutubeRoom.Entities;

/// <summary> 
/// Класс, представляющий зрителя YouTube комнаты. 
/// </summary> 
public class YoutubeViewer : Viewer
{
    /// <summary> 
    /// Создает новый объект класса YoutubeViewer. 
    /// </summary> 
    /// <param name="viewer">DTO объект зрителя.</param> 
    /// <param name="id">Идентификатор зрителя.</param> 
    /// <param name="currentVideoId">Идентификатор текущего видео.</param> 
    internal YoutubeViewer(ViewerDto viewer, int id, string currentVideoId) : base(id, viewer)
    {
        CurrentVideoId = currentVideoId;
    }

    /// <summary> 
    /// Получает или устанавливает идентификатор текущего видео. 
    /// </summary> 
    public string CurrentVideoId { get; set; }
}