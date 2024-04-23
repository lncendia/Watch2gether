namespace Films.Application.Abstractions.Exceptions;

public class PosterMissingException() : Exception("The film or playlist can't be without a poster");