namespace Overoom.Application.Abstractions.Users.Exceptions;

public class ThumbnailSaveException : Exception
{
    public ThumbnailSaveException(Exception ex) : base("Failed to save thumbnail", ex)
    {
    }
}