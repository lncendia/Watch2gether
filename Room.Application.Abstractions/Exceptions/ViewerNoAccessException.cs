namespace Room.Application.Abstractions.Exceptions;

public class ViewerNoAccessException() : Exception("The viewer is not in the room");