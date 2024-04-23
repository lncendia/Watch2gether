using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Messages.FilmMessages;
using Room.Domain.Messages.FilmMessages.Ordering.Visitor;
using Room.Domain.Messages.FilmMessages.Specifications.Visitor;
using Room.Domain.Rooms.FilmRooms;
using Room.Domain.Rooms.FilmRooms.Ordering.Visitor;
using Room.Domain.Rooms.FilmRooms.Specifications.Visitor;

namespace Room.Domain.Abstractions.Repositories;

public interface IFilmMessageRepository : IRepository<FilmMessage, Guid, IFilmMessageSpecificationVisitor, IFilmMessageSortingVisitor>;