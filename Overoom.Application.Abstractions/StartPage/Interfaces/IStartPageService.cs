using Overoom.Application.Abstractions.StartPage.DTOs;

namespace Overoom.Application.Abstractions.StartPage.Interfaces;

public interface IStartPageService
{
    Task<IReadOnlyCollection<CommentStartPageDto>> GetCommentsAsync();
    Task<IReadOnlyCollection<RoomStartPageDto>> GetRoomsAsync();
    Task<IReadOnlyCollection<FilmStartPageDto>> GetFilmsAsync();
}