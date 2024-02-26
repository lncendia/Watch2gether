namespace Films.Domain.Playlists.Exceptions;

public class DescriptionLengthException()
    : Exception("The playlist description length must be between 1 and 500 characters");