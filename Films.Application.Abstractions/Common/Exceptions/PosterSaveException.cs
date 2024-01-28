namespace Films.Application.Abstractions.Common.Exceptions;

public class PosterSaveException(Exception ex) : Exception("Failed to save poster", ex);