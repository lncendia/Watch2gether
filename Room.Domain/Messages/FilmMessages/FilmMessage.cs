using Room.Domain.Messages.Messages;
using Room.Domain.Rooms.FilmRooms;

namespace Room.Domain.Messages.FilmMessages;

public class FilmMessage(FilmRoom room, Guid userId, string text)
    : Message(room.Viewers, room.Id, userId, text);