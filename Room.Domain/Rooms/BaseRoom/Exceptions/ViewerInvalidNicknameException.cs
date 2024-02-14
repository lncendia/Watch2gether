namespace Room.Domain.Rooms.BaseRoom.Exceptions;

public class ViewerInvalidNicknameException() : Exception(
    "Nickname must be between 1 and 20 characters long and can contain only latin or cyrillic letters, digits and underscores.");