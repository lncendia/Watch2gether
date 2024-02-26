namespace PJMS.AuthService.Abstractions.Exceptions;

/// <summary>
/// Исключение, возникающее при ошибке сохранения миниатюры.
/// </summary>
public class ThumbnailSaveException(Exception ex) : Exception("Failed to save thumbnail", ex);