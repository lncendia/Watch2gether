namespace Films.Application.Abstractions.Common.Exceptions;

public class PosterMissingException() : Exception("The film or playlist can't be without a poster");