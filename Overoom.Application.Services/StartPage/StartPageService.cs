using Overoom.Application.Abstractions.StartPage.DTOs;
using Overoom.Application.Abstractions.StartPage.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Comments.Ordering;
using Overoom.Domain.Comments.Ordering.Visitor;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Films.Ordering;
using Overoom.Domain.Films.Ordering.Visitor;
using Overoom.Domain.Films.Specifications;
using Overoom.Domain.Films.Specifications.Visitor;
using Overoom.Domain.Ordering;
using Overoom.Domain.Rooms.FilmRoom.Entities;
using Overoom.Domain.Rooms.FilmRoom.Ordering;
using Overoom.Domain.Rooms.FilmRoom.Ordering.Visitor;
using Overoom.Domain.Rooms.YoutubeRoom.Entities;
using Overoom.Domain.Rooms.YoutubeRoom.Ordering.Visitor;
using Overoom.Domain.Specifications;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.Users.Specifications;
using Overoom.Domain.Users.Specifications.Visitor;

namespace Overoom.Application.Services.StartPage;

public class StartPageService : IStartPageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStartPageMapper _mapper;

    public StartPageService(IUnitOfWork unitOfWork, IStartPageMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<FilmDto>> GetFilmsAsync()
    {
        var films = await _unitOfWork.FilmRepository.Value.FindAsync(null,
            new DescendingOrder<Film, IFilmSortingVisitor>(new FilmOrderByDate()), take: 10);
        return films.Select(_mapper.MapFilm).ToList();
    }

    public async Task<IReadOnlyCollection<CommentDto>> GetCommentsAsync()
    {
        var comments = await _unitOfWork.CommentRepository.Value.FindAsync(null,
            new DescendingOrder<Domain.Comments.Entities.Comment, ICommentSortingVisitor>(
                new CommentOrderByDate()), take: 20);

        ISpecification<Domain.Users.Entities.User, IUserSpecificationVisitor>? spec = null;

        foreach (var comment in comments.Where(x => x.UserId.HasValue))
        {
            spec = AddToSpecification(spec, new UserByIdSpecification(comment.UserId!.Value));
        }

        var users = await _unitOfWork.UserRepository.Value.FindAsync(spec);

        return comments.Select(x => _mapper.MapComment(x, users.FirstOrDefault(u => u.Id == x.UserId))).ToList();
    }

    public async Task<IReadOnlyCollection<RoomDto>> GetRoomsAsync()
    {
        var frooms = _unitOfWork.FilmRoomRepository.Value.FindAsync(null,
            new DescendingOrder<FilmRoom, IFilmRoomSortingVisitor>(new FilmRoomOrderByLastActivityDate()), take: 5);
        var yrooms = _unitOfWork.YoutubeRoomRepository.Value.FindAsync(null,
            new DescendingOrder<YoutubeRoom, IYoutubeRoomSortingVisitor>(
                new Domain.Rooms.YoutubeRoom.Ordering.YoutubeRoomOrderByLastActivityDate()), take: 5);

        await Task.WhenAll(frooms, yrooms);

        ISpecification<Film, IFilmSpecificationVisitor>? spec = null;
        foreach (var filmRoom in frooms.Result)
        {
            spec = AddToSpecification(spec, new FilmByIdSpecification(filmRoom.FilmId));
        }

        var films = await _unitOfWork.FilmRepository.Value.FindAsync(spec);

        var filmRooms = frooms.Result.Select(x => _mapper.MapFilmRoom(x, films.First(f => f.Id == x.FilmId)));

        var youtubeRooms = yrooms.Result.Select(_mapper.MapYoutubeRoom);

        return filmRooms.Concat(youtubeRooms).OrderByDescending(x => x.CountUsers).ToList();
    }


    private static ISpecification<Domain.Users.Entities.User, IUserSpecificationVisitor> AddToSpecification(
        ISpecification<Domain.Users.Entities.User, IUserSpecificationVisitor>? baseSpec,
        ISpecification<Domain.Users.Entities.User, IUserSpecificationVisitor> newSpec)
    {
        return baseSpec == null
            ? newSpec
            : new AndSpecification<Domain.Users.Entities.User, IUserSpecificationVisitor>(baseSpec, newSpec);
    }

    private static ISpecification<Film, IFilmSpecificationVisitor> AddToSpecification(
        ISpecification<Film, IFilmSpecificationVisitor>? baseSpec,
        ISpecification<Film, IFilmSpecificationVisitor> newSpec)
    {
        return baseSpec == null
            ? newSpec
            : new AndSpecification<Film, IFilmSpecificationVisitor>(baseSpec, newSpec);
    }
}