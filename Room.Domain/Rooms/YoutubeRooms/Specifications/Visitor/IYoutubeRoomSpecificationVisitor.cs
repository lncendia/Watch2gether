using Room.Domain.Specifications.Abstractions;

namespace Room.Domain.Rooms.YoutubeRooms.Specifications.Visitor;

public interface IYoutubeRoomSpecificationVisitor : ISpecificationVisitor<IYoutubeRoomSpecificationVisitor, YoutubeRoom>;