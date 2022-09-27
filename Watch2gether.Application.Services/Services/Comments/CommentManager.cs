using Watch2gether.Application.Abstractions;
using Watch2gether.Application.Abstractions.DTO.Comments;
using Watch2gether.Application.Abstractions.Exceptions.Films;
using Watch2gether.Application.Abstractions.Exceptions.Users;
using Watch2gether.Application.Abstractions.Interfaces.Comments;
using Watch2gether.Domain.Abstractions.Repositories.UnitOfWorks;
using Watch2gether.Domain.Comments;
using Watch2gether.Domain.Comments.Ordering;
using Watch2gether.Domain.Comments.Ordering.Visitor;
using Watch2gether.Domain.Comments.Specifications;
using Watch2gether.Domain.Ordering;
using Watch2gether.Domain.Specifications;
using Watch2gether.Domain.Specifications.Abstractions;
using Watch2gether.Domain.Users;
using Watch2gether.Domain.Users.Specifications;
using Watch2gether.Domain.Users.Specifications.Visitor;

namespace Watch2gether.Application.Services.Services.Comments;

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
}