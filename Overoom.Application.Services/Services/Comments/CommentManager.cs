using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.DTO.Comments;
using Overoom.Application.Abstractions.Exceptions.Comments;
using Overoom.Application.Abstractions.Exceptions.Films;
using Overoom.Application.Abstractions.Exceptions.Users;
using Overoom.Application.Abstractions.Interfaces.Comments;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Comments;
using Overoom.Domain.Comments.Ordering;
using Overoom.Domain.Comments.Ordering.Visitor;
using Overoom.Domain.Comments.Specifications;
using Overoom.Domain.Ordering;
using Overoom.Domain.Specifications;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.Users;
using Overoom.Domain.Users.Specifications;
using Overoom.Domain.Users.Specifications.Visitor;

namespace Overoom.Application.Services.Services.Comments;

public class CommentManager : ICommentManager
{
    private readonly IUnitOfWork _unitOfWork;

    public CommentManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<CommentDto>> GetCommentsAsync(Guid filmId, int page)
    {
        var comments = await _unitOfWork.CommentRepository.Value.FindAsync(new FilmCommentsSpecification(filmId),
            new DescendingOrder<Comment, ICommentSortingVisitor>(new OrderByDate()), (page - 1) * 10, 10);
        if (!comments.Any()) return new List<CommentDto>();
        ISpecification<User, IUserSpecificationVisitor> spec = new UserByIdSpecification(comments[0].UserId);
        for (var i = 1; i < comments.Count; i++)
        {
            spec = new OrSpecification<User, IUserSpecificationVisitor>(spec,
                new UserByIdSpecification(comments[i].UserId));
        }

        var users = await _unitOfWork.UserRepository.Value.FindAsync(spec);
        return Map(comments, users);
    }

    private static List<CommentDto> Map(IEnumerable<Comment> comments, IEnumerable<User> users) =>
        (from comment in comments
            let user = users.FirstOrDefault(x => x.Id == comment.UserId)
            select new CommentDto(comment.Id, comment.Text, comment.CreatedAt, user?.Name ?? "Удаленный пользователь",
                user?.AvatarFileName ?? ApplicationConstants.DefaultAvatar)).ToList();

    public async Task<CommentDto> AddCommentAsync(Guid filmId, string email, string text)
    {
        var t1 = _unitOfWork.UserRepository.Value.FindAsync(new UserByEmailSpecification(email));
        var t2 = _unitOfWork.FilmRepository.Value.GetAsync(filmId);
        await Task.WhenAll(t1, t2);
        var user = t1.Result.FirstOrDefault();
        var film = t2.Result;
        if (user == null) throw new UserNotFoundException();
        if (film == null) throw new FilmNotFoundException();
        var comment = new Comment(filmId, user.Id, text);
        await _unitOfWork.CommentRepository.Value.AddAsync(comment);
        await _unitOfWork.SaveAsync();
        return new CommentDto(comment.Id, comment.Text, comment.CreatedAt, user.Name, user.AvatarFileName);
    }

    public async Task UserDeleteCommentAsync(Guid commentId, string email)
    {
        var t1 = _unitOfWork.UserRepository.Value.FindAsync(new UserByEmailSpecification(email));
        var t2 = _unitOfWork.CommentRepository.Value.GetAsync(commentId);
        await Task.WhenAll(t1, t2);
        var user = t1.Result.FirstOrDefault();
        var comment = t2.Result;
        if (user == null) throw new UserNotFoundException();
        if (comment == null) throw new CommentNotFoundException();
        if (comment.UserId != user.Id) throw new CommentNotBelongToUserException();
        await _unitOfWork.CommentRepository.Value.DeleteAsync(comment);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteCommentAsync(Guid commentId)
    {
        var comment = await _unitOfWork.CommentRepository.Value.GetAsync(commentId);
        if (comment == null) throw new CommentNotFoundException();
        await _unitOfWork.CommentRepository.Value.DeleteAsync(comment);
        await _unitOfWork.SaveAsync();
    }
}