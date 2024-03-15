namespace Room.Domain.Rooms.Rooms.Exceptions;

public class InvalidUsernameLengthException() : Exception("The username length must be between 1 and 200 characters");