namespace Films.Application.Abstractions.Exceptions;

public class PosterSaveException(Exception ex) : Exception("Failed to save poster", ex);