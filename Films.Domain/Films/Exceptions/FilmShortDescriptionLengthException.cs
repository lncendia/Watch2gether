namespace Films.Domain.Films.Exceptions;

public class FilmShortDescriptionLengthException()
    : Exception("The movie short description length must be between 1 and 500 characters");