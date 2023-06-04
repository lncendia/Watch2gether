using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.Comment.DTOs;
using Overoom.Application.Abstractions.Comment.Exceptions;
using Overoom.Application.Abstractions.Comment.Interfaces;
using Overoom.Application.Abstractions.Film.Exceptions;
using Overoom.Application.Abstractions.User.Exceptions;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Comment.Ordering;
using Overoom.Domain.Comment.Ordering.Visitor;
using Overoom.Domain.Comment.Specifications;
using Overoom.Domain.Ordering;
using Overoom.Domain.Specifications;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.User.Specifications;
using Overoom.Domain.User.Specifications.Visitor;

namespace Overoom.Application.Services.Comment;

public class CommentManager : ICommentManager
{
    private readonly IUnitOfWork _unitOfWork;

    public CommentManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<CommentDto>> GetAsync(Guid filmId, int page)
    {
        var comments = await _unitOfWork.CommentRepository.Value.FindAsync(new FilmCommentsSpecification(filmId),
            new DescendingOrder<Domain.Comment.Entities.Comment, ICommentSortingVisitor>(new OrderByDate()), (page - 1) * 10, 10);
        if (!comments.Any()) return new List<CommentDto>();
        ISpecification<Domain.User.Entities.User, IUserSpecificationVisitor> spec = new UserByIdSpecification(comments[0].UserId);
        for (var i = 1; i < comments.Count; i++)
        {
            spec = new OrSpecification<Domain.User.Entities.User, IUserSpecificationVisitor>(spec,
                new UserByIdSpecification(comments[i].UserId));
        }

        var users = await _unitOfWork.UserRepository.Value.FindAsync(spec);
        return Map(comments, users);
    }

    private static List<CommentDto> Map(IEnumerable<Domain.Comment.Entities.Comment> comments, IEnumerable<Domain.User.Entities.User> users) =>
        (from comment in comments
            let user = users.FirstOrDefault(x => x.Id == comment.UserId)
            select new CommentDto(comment.Id, comment.Text, comment.CreatedAt, user?.Name ?? "Удаленный пользователь",
                user?.AvatarFileName ?? ApplicationConstants.DefaultAvatar)).ToList();

    public async Task<CommentDto> AddAsync(Guid filmId, Guid id, string text)
    {
        var user = await _unitOfWork.UserRepository.Value.GetAsync(id);
        if (user == null) throw new UserNotFoundException();
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(filmId);
        if (film == null) throw new FilmNotFoundException();
        var comment = new Domain.Comment.Entities.Comment(filmId, user.Id, text);
        await _unitOfWork.CommentRepository.Value.AddAsync(comment);
        await _unitOfWork.SaveAsync();
        return new CommentDto(comment.Id, comment.Text, comment.CreatedAt, user.Name, user.AvatarFileName);
    }

    public async Task DeleteAsync(Guid commentId, Guid id)
    {
        var user = await _unitOfWork.UserRepository.Value.GetAsync(id);
        if (user == null) throw new UserNotFoundException();
        var comment = await _unitOfWork.CommentRepository.Value.GetAsync(commentId);
        if (comment == null) throw new CommentNotFoundException();
        if (comment.UserId != user.Id) throw new CommentNotBelongToUserException();
        await _unitOfWork.CommentRepository.Value.DeleteAsync(comment);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(Guid commentId)
    {
        var comment = await _unitOfWork.CommentRepository.Value.GetAsync(commentId);
        if (comment == null) throw new CommentNotFoundException();
        await _unitOfWork.CommentRepository.Value.DeleteAsync(comment);
        await _unitOfWork.SaveAsync();
    }
}