namespace Room.Domain.Films.Exceptions;

public class NameLengthException() : Exception("The movie name length must be between 1 and 200 characters");