using Overoom.Application.Abstractions.Comments.DTOs;

namespace Overoom.Application.Abstractions.Comments.Interfaces;

public interface ICommentManager
{
    Task<List<CommentDto>> GetAsync(Guid filmId, int page);
    Task<CommentDto> AddAsync(Guid filmId, Guid id, string text);
    Task DeleteAsync(Guid commentId, Guid id);
    Task DeleteAsync(Guid commentId);
}