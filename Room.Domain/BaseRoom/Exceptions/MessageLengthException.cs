namespace Room.Domain.BaseRoom.Exceptions;

public class MessageLengthException() : Exception("Message length must be between 1 and 1000 characters");