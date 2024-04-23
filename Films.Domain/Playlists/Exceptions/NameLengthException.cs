namespace Films.Domain.Playlists.Exceptions;

public class NameLengthException() : Exception("The playlist name length must be between 1 and 200 characters");