namespace Room.Application.Abstractions.Common.Exceptions;

public class FilmAlreadyExistsException()
    : Exception("There can't be films with the same title and the same release year");