using Overoom.Application.Abstractions.StartPage.DTOs;

namespace Overoom.Application.Abstractions.StartPage.Interfaces;

public interface IStartPageService
{
    Task<IReadOnlyCollection<CommentDto>> GetCommentsAsync();
    Task<IReadOnlyCollection<RoomDto>> GetRoomsAsync();
    Task<IReadOnlyCollection<FilmDto>> GetFilmsAsync();
}