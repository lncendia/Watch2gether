using Room.Domain.Ordering.Abstractions;

namespace Room.Domain.Rooms.YoutubeRooms.Ordering.Visitor;

public interface IYoutubeRoomSortingVisitor : ISortingVisitor<IYoutubeRoomSortingVisitor, YoutubeRoom>;