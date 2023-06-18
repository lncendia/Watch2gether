using Overoom.Application.Abstractions.Comments.DTOs;
using Overoom.Application.Abstractions.Comments.Exceptions;
using Overoom.Application.Abstractions.Comments.Interfaces;
using Overoom.Application.Abstractions.Films.Catalog.Exceptions;
using Overoom.Application.Abstractions.Users.Exceptions;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Comments.Ordering;
using Overoom.Domain.Comments.Ordering.Visitor;
using Overoom.Domain.Comments.Specifications;
using Overoom.Domain.Ordering;
using Overoom.Domain.Specifications;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.Users.Specifications;
using Overoom.Domain.Users.Specifications.Visitor;

namespace Overoom.Application.Services.Comment;

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
        var sorting = new DescendingOrder<Domain.Comments.Entities.Comment, ICommentSortingVisitor>(new OrderByDate());
        var comments = await _unitOfWork.CommentRepository.Value.FindAsync(specification, sorting, (page - 1) * 10, 10);
        if (!comments.Any()) return new List<CommentDto>();

        ISpecification<Domain.Users.Entities.User, IUserSpecificationVisitor>? spec = null;
        foreach (var comment in comments.Where(x => x.UserId.HasValue))
        {
            spec = AddToSpecification(spec, new UserByIdSpecification(comment.UserId!.Value));
        }

        var users = await _unitOfWork.UserRepository.Value.FindAsync(spec);
        return Map(comments, users);
    }

    public async Task<CommentDto> AddAsync(Guid filmId, Guid id, string text)
    {
        var user = await _unitOfWork.UserRepository.Value.GetAsync(id);
        if (user == null) throw new UserNotFoundException();
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(filmId);
        if (film == null) throw new FilmNotFoundException();
        var comment = new Domain.Comments.Entities.Comment(filmId, user.Id, text);
        await _unitOfWork.CommentRepository.Value.AddAsync(comment);
        await _unitOfWork.SaveChangesAsync();
        return new CommentDto(comment.Id, comment.Text, comment.CreatedAt, user.Name, user.AvatarUri);
    }

    public async Task DeleteAsync(Guid commentId, Guid id)
    {
        var comment = await _unitOfWork.CommentRepository.Value.GetAsync(commentId);
        if (comment == null) throw new CommentNotFoundException();
        if (comment.UserId != id) throw new CommentNotBelongToUserException();
        await _unitOfWork.CommentRepository.Value.DeleteAsync(comment.Id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid commentId)
    {
        await _unitOfWork.CommentRepository.Value.DeleteAsync(commentId);
        await _unitOfWork.SaveChangesAsync();
    }


    private static ISpecification<Domain.Users.Entities.User, IUserSpecificationVisitor> AddToSpecification(
        ISpecification<Domain.Users.Entities.User, IUserSpecificationVisitor>? baseSpec,
        ISpecification<Domain.Users.Entities.User, IUserSpecificationVisitor> newSpec)
    {
        return baseSpec == null
            ? newSpec
            : new AndSpecification<Domain.Users.Entities.User, IUserSpecificationVisitor>(baseSpec, newSpec);
    }

    private List<CommentDto> Map(IList<Domain.Comments.Entities.Comment> comments,
        IList<Domain.Users.Entities.User> users)
    {
        return (from comment in comments
            let user = users.FirstOrDefault(x => x.Id == comment.UserId)
            select _mapper.Map(comment, user)).ToList();
    }
}