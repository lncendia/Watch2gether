using Overoom.Application.Abstractions.Comments.DTOs;
using Overoom.Application.Abstractions.Comments.Exceptions;
using Overoom.Application.Abstractions.Comments.Interfaces;
using Overoom.Application.Abstractions.Common.Exceptions;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Comments.Entities;
using Overoom.Domain.Comments.Ordering;
using Overoom.Domain.Comments.Ordering.Visitor;
using Overoom.Domain.Comments.Specifications;
using Overoom.Domain.Ordering;
using Overoom.Domain.Users.Specifications;

namespace Overoom.Application.Services.Comments;

public class CommentManager : ICommentManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommentMapper _mapper;

    public CommentManager(IUnitOfWork unitOfWork, ICommentMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CommentDto>> GetAsync(Guid filmId, int page)
    {
        var specification = new FilmCommentsSpecification(filmId);
        var sorting = new DescendingOrder<Comment, ICommentSortingVisitor>(new CommentOrderByDate());
        var comments = await _unitOfWork.CommentRepository.Value.FindAsync(specification, sorting, (page - 1) * 10, 10);
        if (!comments.Any()) return new List<CommentDto>();

        var spec = new UserByIdsSpecification(comments.Select(x => x.UserId));

        var users = await _unitOfWork.UserRepository.Value.FindAsync(spec);
        return comments.Select(c => _mapper.Map(c, users.First(u => u.Id == c.UserId))).ToList();
    }

    public async Task<CommentDto> AddAsync(Guid filmId, Guid id, string text)
    {
        var user = await _unitOfWork.UserRepository.Value.GetAsync(id);
        if (user == null) throw new UserNotFoundException();
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(filmId);
        if (film == null) throw new FilmNotFoundException();
        var comment = new Comment(filmId, user.Id, text);
        await _unitOfWork.CommentRepository.Value.AddAsync(comment);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map(comment, user);
    }

    public async Task DeleteAsync(Guid commentId, Guid id)
    {
        var comment = await _unitOfWork.CommentRepository.Value.GetAsync(commentId);
        if (comment == null) throw new CommentNotFoundException();
        if (comment.UserId != id) throw new CommentNotBelongToUserException();
        await _unitOfWork.CommentRepository.Value.DeleteAsync(comment.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}