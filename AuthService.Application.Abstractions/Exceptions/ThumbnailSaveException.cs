namespace AuthService.Application.Abstractions.Exceptions;

public class ThumbnailSaveException(Exception ex) : Exception("Failed to save thumbnail", ex);