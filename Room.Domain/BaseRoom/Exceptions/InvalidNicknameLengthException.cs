namespace Room.Domain.BaseRoom.Exceptions;

public class InvalidNicknameLengthException() : Exception("The nickname length must be between 1 and 200 characters");