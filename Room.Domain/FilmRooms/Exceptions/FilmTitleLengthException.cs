namespace Room.Domain.FilmRooms.Exceptions;

public class FilmTitleLengthException() : Exception("The movie name length must be between 1 and 200 characters");