using Room.Domain.Messages.Messages;
using Room.Domain.Rooms.YoutubeRooms;

namespace Room.Domain.Messages.YoutubeMessages;

public class YoutubeMessage(YoutubeRoom room, Guid userId, string text)
    : Message(room.Viewers, room.Id, userId, text);